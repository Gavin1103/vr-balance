package vr.balance.app.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import vr.balance.app.models.User;

import java.util.Optional;

public interface UserRepository extends JpaRepository<User, Long> {
    Optional<User> findByEmail(String email);

    Optional<User> findById(Long id);

    Optional<User> findByUsername(String username);

    Boolean existsByEmail(String email);

    Boolean existsByUsername(String user);

}
