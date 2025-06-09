package vr.balance.app.unit.service;

import org.junit.jupiter.api.Test;
import org.modelmapper.ModelMapper;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.crypto.password.PasswordEncoder;
import vr.balance.app.API.request.user.ChangePasswordDTO;
import vr.balance.app.exceptions.InvalidCredentialsException;
import vr.balance.app.exceptions.InvalidPasswordException;
import vr.balance.app.exceptions.NotFoundException;
import vr.balance.app.exceptions.PasswordMismatchException;
import vr.balance.app.models.User;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.security.JwtService;
import vr.balance.app.security.PincodeAuthenticationToken;
import vr.balance.app.service.AuthenticationService;
import vr.balance.app.service.EmailService;

import java.util.Optional;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.mockito.Mockito.*;

public class AuthenticationServiceTest {
    private final AuthenticationManager authenticationManager = mock(AuthenticationManager.class);
    private final UserRepository userRepository = mock(UserRepository.class);
    private final JwtService jwtService = mock(JwtService.class);
    private final PasswordEncoder passwordEncoder = mock(PasswordEncoder.class);
    private final ModelMapper modelMapper = new ModelMapper();
    private final EmailService emailService = mock(EmailService.class);

    private final AuthenticationService authenticationService = new AuthenticationService(
            authenticationManager,
            userRepository,
            jwtService,
            passwordEncoder,
            modelMapper,
            emailService
    );
    @Test
    void login_shouldSucceed() {
        // ---------- ARRANGE ----------

        // Stel de testinput in: e-mailadres en wachtwoord die de gebruiker invoert
        String email = "email@example.com";
        String password = "password";

        // Simuleer een bestaande gebruiker in de database
        User user = new User();
        user.setId(1L);
        user.setEmail(email);
        user.setPassword("hashedPassword"); // In echte situatie zou dit met BCrypt geverifieerd worden

        // Maak een authentication token aan zoals Spring Security dat zou doen
        UsernamePasswordAuthenticationToken authToken =
                new UsernamePasswordAuthenticationToken(email, password);

        // Mock de Authentication object dat normaal door Spring wordt teruggegeven
        Authentication authenticationMock = mock(Authentication.class);

        // Definieer gedrag van de mocks:
        // - AuthenticationManager valideert de credentials
        when(authenticationManager.authenticate(authToken)).thenReturn(authenticationMock);
        // - Gebruiker wordt gevonden op basis van e-mailadres
        when(userRepository.findByEmail(email)).thenReturn(Optional.of(user));
        // - JwtService genereert een token op basis van de gebruiker en rol
        when(jwtService.generateToken(user, "STANDARD")).thenReturn("mocked-jwt-token");

        // ---------- ACT ----------

        // Roep de methode aan die getest wordt
        String token = authenticationService.login(email, password);

        // ---------- ASSERT ----------

        // Controleer of het resultaat het verwachte token is
        assertEquals("mocked-jwt-token", token);

        // Verifieer of de juiste methodes zijn aangeroepen met de juiste parameters
        verify(authenticationManager).authenticate(authToken);
        verify(userRepository).findByEmail(email);
        verify(jwtService).generateToken(user, "STANDARD");
    }

    @Test
    void login_shouldThrowInvalidCredentialsException_whenCredentialsAreInvalid() {
        // Arrange
        String email = "email@example.com";
        String password = "wrongPassword";

        UsernamePasswordAuthenticationToken authToken =
                new UsernamePasswordAuthenticationToken(email, password);

        doThrow(new BadCredentialsException("Bad credentials"))
                .when(authenticationManager).authenticate(authToken);

        // Act & Assert
        assertThrows(InvalidCredentialsException.class, () -> {
            authenticationService.login(email, password);
        });

        verify(authenticationManager).authenticate(authToken);
        verify(userRepository, never()).findByEmail(any());
        verify(jwtService, never()).generateToken(any(), any());
    }

    @Test
    void login_shouldThrowNotFoundException_whenUserNotFound() {
        // Arrange
        String email = "email@example.com";
        String password = "validPassword";

        UsernamePasswordAuthenticationToken authToken = new UsernamePasswordAuthenticationToken(email, password);

        when(authenticationManager.authenticate(authToken)).thenReturn(mock(Authentication.class));
        when(userRepository.findByEmail(email)).thenReturn(Optional.empty());

        // Act & Assert
        assertThrows(NotFoundException.class, () -> {
            authenticationService.login(email, password);
        });

        verify(authenticationManager).authenticate(authToken);
        verify(userRepository).findByEmail(email);
        verify(jwtService, never()).generateToken(any(), any());
    }

    @Test
    void loginWithPincode_shouldSucceed() {
        // Arrange
        String identifier = "john_doe";
        String pincode = "1234";

        User user = new User();
        user.setId(1L);
        user.setUsername(identifier);
        user.setPincode("hashedPincode");

        Authentication auth = new PincodeAuthenticationToken(identifier, pincode);

        when(userRepository.findByEmail(identifier)).thenReturn(Optional.empty());
        when(userRepository.findByUsername(identifier)).thenReturn(Optional.of(user));
        when(jwtService.generateToken(user, "PINCODE")).thenReturn("mocked-jwt-token");
        when(authenticationManager.authenticate(auth)).thenReturn(mock(Authentication.class));

        // Act
        String token = authenticationService.loginWithPincode(identifier, pincode);

        // Assert
        assertEquals("mocked-jwt-token", token);
        verify(authenticationManager).authenticate(auth);
        verify(userRepository).findByEmail(identifier);
        verify(userRepository).findByUsername(identifier);
        verify(jwtService).generateToken(user, "PINCODE");
    }

    @Test
    void loginWithPincode_shouldThrowInvalidCredentialsException_whenAuthenticationFails() {
        // Arrange
        String identifier = "john_doe";
        String pincode = "wrongPincode";

        Authentication auth = new PincodeAuthenticationToken(identifier, pincode);

        doThrow(new BadCredentialsException("Bad credentials"))
                .when(authenticationManager).authenticate(auth);

        // Act & Assert
        assertThrows(InvalidCredentialsException.class, () -> {
            authenticationService.loginWithPincode(identifier, pincode);
        });

        verify(authenticationManager).authenticate(auth);
        verify(userRepository, never()).findByEmail(any());
        verify(userRepository, never()).findByUsername(any());
        verify(jwtService, never()).generateToken(any(), any());
    }

    @Test
    void loginWithPincode_shouldThrowNotFoundException_whenUserNotFound() {
        // Arrange
        String identifier = "john_doe";
        String pincode = "1234";

        Authentication auth = new PincodeAuthenticationToken(identifier, pincode);

        when(authenticationManager.authenticate(auth)).thenReturn(mock(Authentication.class));
        when(userRepository.findByEmail(identifier)).thenReturn(Optional.empty());
        when(userRepository.findByUsername(identifier)).thenReturn(Optional.empty());

        // Act & Assert
        assertThrows(NotFoundException.class, () -> {
            authenticationService.loginWithPincode(identifier, pincode);
        });

        verify(authenticationManager).authenticate(auth);
        verify(userRepository).findByEmail(identifier);
        verify(userRepository).findByUsername(identifier);
        verify(jwtService, never()).generateToken(any(), any());
    }

    @Test
    void changePassword_shouldUpdatePasswordSuccessfully() {
        // Arange
        User user = new User();
        user.setId(1L);
        user.setPassword("hashedPassword");

        ChangePasswordDTO changePasswordDTO = ChangePasswordDTO.builder()
                .currentPassword("currentPassword")
                .newPassword("newPassword")
                .repeatNewPassword("newPassword")
                .build();

        // Act
        when(userRepository.findById(1L)).thenReturn(Optional.of(user));
        when(passwordEncoder.matches("currentPassword", "hashedPassword")).thenReturn(true);
        when(passwordEncoder.matches("newPassword", "hashedPassword")).thenReturn(false);
        when(passwordEncoder.encode("newPassword")).thenReturn("encodedNewPassword");

        authenticationService.changePassword(changePasswordDTO, user.getId());

        // Assert
        assertEquals("encodedNewPassword", user.getPassword());

        verify(userRepository).save(user);
    }

    @Test
    void changePassword_shouldThrowExceptionWhenCurrentPasswordIsIncorrect() {
        // Arrange
        User user = new User();
        user.setId(1L);
        user.setPassword("hashedPassword");

        ChangePasswordDTO changePasswordDTO = ChangePasswordDTO.builder()
                .currentPassword("wrongPassword")
                .newPassword("newPassword")
                .repeatNewPassword("newPassword")
                .build();

        when(userRepository.findById(1L)).thenReturn(Optional.of(user));
        when(passwordEncoder.matches("wrongPassword", "hashedPassword")).thenReturn(false);

        // Act & Assert
        assertThrows(InvalidPasswordException.class, () -> {
            authenticationService.changePassword(changePasswordDTO, user.getId());
        });

        verify(userRepository, never()).save(any());
    }

    @Test
    void changePassword_shouldThrowExceptionWhenNewPasswordIsSameAsCurrent() {
        // Arrange
        User user = new User();
        user.setId(1L);
        user.setPassword("hashedPassword");

        ChangePasswordDTO changePasswordDTO = ChangePasswordDTO.builder()
                .currentPassword("currentPassword")
                .newPassword("currentPassword")
                .repeatNewPassword("currentPassword")
                .build();

        when(userRepository.findById(1L)).thenReturn(Optional.of(user));
        when(passwordEncoder.matches("currentPassword", "hashedPassword")).thenReturn(true);
        when(passwordEncoder.matches("currentPassword", "hashedPassword")).thenReturn(true);

        // Act & Assert
        assertThrows(PasswordMismatchException.class, () -> {
            authenticationService.changePassword(changePasswordDTO, user.getId());
        });

        verify(userRepository, never()).save(any());
    }

    @Test
    void changePassword_shouldThrowExceptionWhenRepeatNewPasswordIsNotTheSameAsNewPassword() {
        // Arrange
        User user = new User();
        user.setId(1L);
        user.setPassword("hashedPassword");

        ChangePasswordDTO changePasswordDTO = ChangePasswordDTO.builder()
                .currentPassword("currentPassword")
                .newPassword("newPassword")
                .repeatNewPassword("newPasswordNotIdentical")
                .build();

        when(userRepository.findById(1L)).thenReturn(Optional.of(user));
        when(passwordEncoder.matches("currentPassword", "hashedPassword")).thenReturn(true);
        when(passwordEncoder.matches("newPassword", "hashedPassword")).thenReturn(false);

        // Act & Assert
        assertThrows(PasswordMismatchException.class, () -> {
            authenticationService.changePassword(changePasswordDTO, user.getId());
        });

        verify(userRepository, never()).save(any());
    }
}
