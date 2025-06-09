package vr.balance.app.seed;

import lombok.RequiredArgsConstructor;
import org.springframework.boot.CommandLineRunner;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Component;
import org.springframework.transaction.annotation.Transactional;
import vr.balance.app.API.request.CompletedBalanceTestExerciseDTO;
import vr.balance.app.API.request.CompletedFireflyExerciseDTO;
import vr.balance.app.enums.DifficultyEnum;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.enums.RoleEnum;
import vr.balance.app.exceptions.NotFoundException;
import vr.balance.app.models.User;
import vr.balance.app.models.exercise.CompletedBalanceTestExercise;
import vr.balance.app.models.exercise.CompletedFireflyExercise;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.repository.UserStatsRepository;
import vr.balance.app.repository.exercise.CompletedBalanceTestExerciseRepository;
import vr.balance.app.repository.exercise.CompletedFireflyExerciseRepository;
import vr.balance.app.seed.record.UserSeedData;
import vr.balance.app.service.CompletedExerciseService;

import java.time.Instant;
import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.List;
import java.util.concurrent.ThreadLocalRandom;

@Component
@RequiredArgsConstructor
public class DatabaseSeeder implements CommandLineRunner {

    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    private final UserStatsRepository userStatsRepository;
    private final CompletedExerciseService completedExerciseService;
    private final CompletedBalanceTestExerciseRepository completedBalanceTestExerciseRepository;
    private final CompletedFireflyExerciseRepository completedFireflyExerciseRepository;


    @Override
    @Transactional
    public void run(String... args) {
        this.createUsers();
//        this.createCompletedFireflyExercises();
//        this.createCompletedBalanceTestExercises();
    }

    private void createUsers() {
        List<UserSeedData> usersToCreate = List.of(
                new UserSeedData("admin@vrbalance.com", "admin", "admin", "0000", RoleEnum.ADMIN),
                new UserSeedData("john@vrbalance.com", "john", "john", "0000", RoleEnum.PATIENT),
                new UserSeedData("gavin@vrbalance.com", "gavin", "gavin", "0000", RoleEnum.PHYSIOTHERAPIST)
        );

        for (UserSeedData data : usersToCreate) {
            if (userRepository.existsByEmail(data.email())) {
                continue;
            }
            User user = User.builder()
                    .email(data.email())
                    .username(data.username())
                    .password(passwordEncoder.encode(data.password()))
                    .pincode(passwordEncoder.encode(data.pincode()))
                    .role(data.role())
                    .createdAt(Instant.now())
                    .birthDate(LocalDate.now())
                    .build();
            userRepository.save(user);
        }
    }

    private void createCompletedBalanceTestExercises() {
        List<User> users = List.of(
                userRepository.findByEmail("admin@vrbalance.com").orElseThrow(() -> new NotFoundException("User1 not found")),
                userRepository.findByEmail("gavin@vrbalance.com").orElseThrow(() -> new NotFoundException("User2 not found")),
                userRepository.findByEmail("john@vrbalance.com").orElseThrow(() -> new NotFoundException("User3 not found"))
        );

        for (User user : users) {
            boolean alreadyExists = completedBalanceTestExerciseRepository.existsByUserIdAndExercise(user.getId(), ExerciseEnum.BALANCE_TEST_EXERCISE);

            if (alreadyExists) {
                continue;
            }

            CompletedBalanceTestExerciseDTO test = CompletedBalanceTestExerciseDTO.builder()
                    .difficulty(DifficultyEnum.EASY)
                    .completedAt(Instant.now())
                    .earnedPoints(0)
                    .phase_1("phase1Data")
                    .phase_2("phase2Data")
                    .phase_3("phase3Data")
                    .phase_4("phase4Data")
                    .build();

            completedExerciseService.saveExercise(
                    CompletedBalanceTestExercise.class,
                    test,
                    user.getId(),
                    ExerciseEnum.BALANCE_TEST_EXERCISE
            );
        }
    }

    private void createCompletedFireflyExercises() {
        User user1 = userRepository.findByEmail("admin@vrbalance.com").orElseThrow(() -> new NotFoundException("User1 not found"));
        User user2 = userRepository.findByEmail("gavin@vrbalance.com").orElseThrow(() -> new NotFoundException("User2 not found"));
        User user3 = userRepository.findByEmail("john@vrbalance.com").orElseThrow(() -> new NotFoundException("User3 not found"));

        saveFireflyExercisesForUser(user1);
        saveFireflyExercisesForUser(user2);
        saveFireflyExercisesForUser(user3);
    }

    private List<CompletedFireflyExerciseDTO> generateRandomFireflyExercises() {
        return List.of(
                CompletedFireflyExerciseDTO.builder()
                        .difficulty(DifficultyEnum.EASY)
                        .earnedPoints(randomInt(100, 300))
                        .completedAt(Instant.now().minus(2, ChronoUnit.DAYS))
                        .caughtFirefliesCount(randomInt(0, 50))
                        .caughtWrongFirefliesCount(randomInt(10, 20))
                        .build(),

                CompletedFireflyExerciseDTO.builder()
                        .difficulty(DifficultyEnum.MEDIUM)
                        .earnedPoints(randomInt(100, 300))
                        .completedAt(Instant.now().minus(1, ChronoUnit.DAYS))
                        .caughtFirefliesCount(randomInt(0, 50))
                        .caughtWrongFirefliesCount(randomInt(10, 20))
                        .build(),

                CompletedFireflyExerciseDTO.builder()
                        .difficulty(DifficultyEnum.HARD)
                        .earnedPoints(randomInt(100, 300))
                        .completedAt(Instant.now())
                        .caughtFirefliesCount(randomInt(0, 50))
                        .caughtWrongFirefliesCount(randomInt(10, 20))
                        .build()
        );
    }

    private int randomInt(int min, int max) {
        return ThreadLocalRandom.current().nextInt(min, max + 1);
    }

    private void saveFireflyExercisesForUser(User user) {
        long existingCount = completedFireflyExerciseRepository.countByUserId(user.getId());

        if (existingCount >= 3) {
            return;
        }

        int exercisesToAdd = (int)(3 - existingCount);
        List<CompletedFireflyExerciseDTO> allExercises = generateRandomFireflyExercises();
        List<CompletedFireflyExerciseDTO> toSave = allExercises.subList(0, exercisesToAdd);

        for (CompletedFireflyExerciseDTO exercise : toSave) {
            completedExerciseService.saveExercise(
                    CompletedFireflyExercise.class,
                    exercise,
                    user.getId(),
                    ExerciseEnum.FIREFLY_EXERCISE
            );
        }
    }
}
