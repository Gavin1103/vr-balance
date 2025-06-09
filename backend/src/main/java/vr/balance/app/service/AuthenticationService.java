package vr.balance.app.service;

import lombok.RequiredArgsConstructor;
import org.modelmapper.ModelMapper;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import vr.balance.app.DTO.request.user.ChangePasswordDTO;
import vr.balance.app.DTO.request.user.ChangePincodeDTO;
import vr.balance.app.DTO.request.user.RegisterPatientDTO;
import vr.balance.app.enums.RoleEnum;
import vr.balance.app.exceptions.*;
import vr.balance.app.models.User;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.security.JwtService;
import vr.balance.app.security.PincodeAuthenticationToken;
import vr.balance.app.util.PasswordGenerator;
import vr.balance.app.util.PincodeGenerator;

import java.time.Instant;
import java.util.function.Consumer;

import static vr.balance.app.enums.AuthenticationMethodEnum.PINCODE;
import static vr.balance.app.enums.AuthenticationMethodEnum.STANDARD;

@Service
@RequiredArgsConstructor
public class AuthenticationService {

    private final AuthenticationManager authManager;
    private final UserRepository userRepository;
    private final JwtService jwtService;
    private final PasswordEncoder passwordEncoder;
    private final ModelMapper modelMapper;
    private final EmailService emailService;

    public Long getCurrentUserId() {
        return Long.parseLong(SecurityContextHolder.getContext().getAuthentication().getName());
    }

    public String login(String email, String password) {
        Authentication auth = new UsernamePasswordAuthenticationToken(email, password);

        try {
            authManager.authenticate(auth);
        } catch (BadCredentialsException e) {
            throw new InvalidCredentialsException("Invalid email or password");
        }

        User user = userRepository.findByEmail(email)
                .orElseThrow(() -> new NotFoundException("User not found"));

        return jwtService.generateToken(user, STANDARD.toString());
    }

    public String loginWithPincode(String identifier, String pincode) {
        Authentication auth = new PincodeAuthenticationToken(identifier, pincode);

        try {
            authManager.authenticate(auth);
        } catch (BadCredentialsException e) {
            throw new InvalidCredentialsException("Invalid username/email or pincode");
        }

        User user = userRepository.findByEmail(identifier)
                .or(() -> userRepository.findByUsername(identifier))
                .orElseThrow(() -> new NotFoundException("User not found"));

        return jwtService.generateToken(user, PINCODE.toString());
    }

    public void registerPatient(RegisterPatientDTO request) {
        if (userRepository.existsByEmail(request.getEmail())) {
            throw new EmailAlreadyInUseException("Email is already in use.");
        }
        if (userRepository.existsByUsername(request.getUsername())) {
            throw new UsernameAlreadyInUseException("Username is already in use.");
        }

        User user = modelMapper.map(request, User.class);
        String generatedPassword = PasswordGenerator.generatePassword(8);
        String generatedPincode = PincodeGenerator.generatePincode();

        user.setPassword(passwordEncoder.encode(generatedPassword));
        user.setPincode(passwordEncoder.encode(generatedPincode));
        user.setRole(RoleEnum.PATIENT);
        user.setCreatedAt(Instant.now());

        userRepository.save(user);

        //Send email
        emailService.sendInstructionEmail(user, generatedPassword, generatedPincode);
    }

    public void changePassword(ChangePasswordDTO request, Long userId) {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new NotFoundException("User not found"));

        updateCredential(
                request.getCurrentPassword(),
                user.getPassword(),
                request.getNewPassword(),
                request.getRepeatNewPassword(),
                user::setPassword,
                "password"
        );

        userRepository.save(user);
    }

    public void changePincode(ChangePincodeDTO request, Long userId) {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new NotFoundException("User not found"));

        updateCredential(
                request.getCurrentPincode(),
                user.getPincode(),
                request.getNewPincode(),
                request.getRepeatNewPincode(),
                user::setPincode,
                "pincode"
        );

        userRepository.save(user);
    }

    /**
     * Validates and updates a credential (e.g., password or pincode) by checking the current value,
     * the new value, and its repetition. If all validations pass, the updateAction is executed
     * with the encoded new value.
     *
     * @param currentValue     The current credential provided by the user (e.g., current password/pincode).
     * @param storedHash       The hashed credential stored in the database.
     * @param newValue         The new credential the user wants to set.
     * @param repeatNewValue   The repeated new credential for confirmation.
     * @param updateAction     A Consumer that performs the update (e.g., user::setPassword).
     * @param type             A string representing the type of credential (used in error messages, e.g., "password" or "pincode").
     * @throws InvalidPasswordException   If the current credential does not match the stored one.
     * @throws PasswordMismatchException  If the new value matches the old one or if the two new values don't match.
     */
    private void updateCredential(
            String currentValue, String storedHash,
            String newValue, String repeatNewValue,
            Consumer<String> updateAction,
            String type
    ) {
        if (!passwordEncoder.matches(currentValue, storedHash)) {
            throw new InvalidPasswordException("Current " + type + " is incorrect.");
        }

        if (passwordEncoder.matches(newValue, storedHash)) {
            throw new PasswordMismatchException("New " + type + " is the same as current " + type + ".");
        }

        if (!newValue.equals(repeatNewValue)) {
            throw new PasswordMismatchException("New " + type + " does not match repeated " + type + ".");
        }

        updateAction.accept(passwordEncoder.encode(newValue));
    }
}
