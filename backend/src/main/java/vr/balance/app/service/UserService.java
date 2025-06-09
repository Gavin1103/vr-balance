package vr.balance.app.service;

import org.modelmapper.ModelMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;
import vr.balance.app.API.request.user.EditUserDTO;
import vr.balance.app.API.response.UserProfileResponse;
import vr.balance.app.exceptions.EmailAlreadyInUseException;
import vr.balance.app.exceptions.NotFoundException;
import vr.balance.app.exceptions.UsernameAlreadyInUseException;
import vr.balance.app.models.User;
import vr.balance.app.repository.UserRepository;

import java.time.Instant;

@Service
public class UserService {

    private final UserRepository userRepository;
    private final ModelMapper modelMapper;

    @Autowired
    public UserService(ModelMapper modelMapper, UserRepository userRepository) {
        this.userRepository = userRepository;
        this.modelMapper = modelMapper;
    }

    public UserProfileResponse getUserProfileById(Long userId) {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new NotFoundException("User not found"));

        return modelMapper.map(user, UserProfileResponse.class);
    }

    public void editUserProfile(EditUserDTO request, Long userId) {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new UsernameNotFoundException("User not found."));

        if (!request.getEmail().equals(user.getEmail()) && userRepository.existsByEmail(request.getEmail())) {
            throw new EmailAlreadyInUseException("Email is already in use.");
        }

        if (!request.getUsername().equals(user.getUsername()) && userRepository.existsByUsername(request.getUsername())) {
            throw new UsernameAlreadyInUseException("Username is already in use.");
        }

        modelMapper.map(request, user);
        user.setUpdatedAt(Instant.now());

        userRepository.save(user);
    }

    public Page<UserProfileResponse> getAllUserProfiles(int page, int size) {
        Pageable pageable = PageRequest.of(page, size);
        Page<User> users = userRepository.findAll(pageable);

        return users.map(user -> modelMapper.map(user, UserProfileResponse.class));
    }
}