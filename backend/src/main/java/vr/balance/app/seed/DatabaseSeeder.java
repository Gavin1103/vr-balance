package vr.balance.app.seed;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import lombok.RequiredArgsConstructor;
import org.springframework.boot.CommandLineRunner;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Component;
import org.springframework.transaction.annotation.Transactional;
import vr.balance.app.DTO.request.CompletedBalanceTestExerciseDTO;
import vr.balance.app.DTO.request.CompletedFireflyExerciseDTO;
import vr.balance.app.DTO.request.Vector3;
import vr.balance.app.enums.DifficultyEnum;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.enums.RoleEnum;
import vr.balance.app.exceptions.NotFoundException;
import vr.balance.app.models.User;
import vr.balance.app.models.exercise.CompletedBalanceTestExercise;
import vr.balance.app.models.exercise.CompletedFireflyExercise;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.repository.exercise.CompletedBalanceTestExerciseRepository;
import vr.balance.app.repository.exercise.CompletedFireflyExerciseRepository;
import vr.balance.app.seed.record.UserSeedData;
import vr.balance.app.service.CompletedExerciseService;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.time.Instant;
import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.ThreadLocalRandom;

@Component
@RequiredArgsConstructor
public class DatabaseSeeder implements CommandLineRunner {

    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    private final CompletedExerciseService completedExerciseService;
    private final CompletedFireflyExerciseRepository completedFireflyExerciseRepository;
    private final CompletedBalanceTestExerciseRepository completedBalanceTestExerciseRepository;

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
                new UserSeedData("tom@vrbalance.com", "Tom", "tom", "0000", RoleEnum.PHYSIOTHERAPIST),

                new UserSeedData("gavin@vrbalance.com", "gavin", "gavin", "0000", RoleEnum.PATIENT),
                new UserSeedData("susant@vrbalance.com", "susant", "susant", "0000", RoleEnum.PATIENT),
                new UserSeedData("patrick@vrbalance.com", "patrick", "patrick", "0000", RoleEnum.PATIENT),
                new UserSeedData("ilias@vrbalance.com", "ililas", "ililas", "0000", RoleEnum.PATIENT),
                new UserSeedData("amine@vrbalance.com", "amine", "amine", "0000", RoleEnum.PATIENT),
                new UserSeedData("hoemeirra@vrbalance.com", "hoemeirra", "hoemeirra", "0000", RoleEnum.PATIENT),

                new UserSeedData("motion@vrbalance.com", "motion", "motion", "0", RoleEnum.PATIENT)
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
                userRepository.findByEmail("gavin@vrbalance.com").orElseThrow(() -> new NotFoundException("User not found")),
                userRepository.findByEmail("susant@vrbalance.com").orElseThrow(() -> new NotFoundException("User not found")),
                userRepository.findByEmail("patrick@vrbalance.com").orElseThrow(() -> new NotFoundException("User not found")),
                userRepository.findByEmail("amine@vrbalance.com").orElseThrow(() -> new NotFoundException("User not found")),
                userRepository.findByEmail("ilias@vrbalance.com").orElseThrow(() -> new NotFoundException("User not found")),
                userRepository.findByEmail("hoemeirra@vrbalance.com").orElseThrow(() -> new NotFoundException("User not found"))
        );

        for (User user : users) {
            long existingCount = completedBalanceTestExerciseRepository.countByUserIdAndExercise(
                    user.getId(), ExerciseEnum.Balance
            );

            if (existingCount >= 5) {
                continue;
            }

            List<Vector3> phaseData1 = loadPhaseData("FakeBalanceTestData1.json");
            List<Vector3> phaseData2 = loadPhaseData("FakeBalanceTestData2.json");
            List<Vector3> phaseData3 = loadPhaseData("FakeBalanceTestData3.json");
            List<Vector3> phaseData4 = loadPhaseData("FakeBalanceTestData4.json");

            List<List<Vector3>> originalPhases = List.of(phaseData1, phaseData2, phaseData3, phaseData4);

            for (int i = 0; i < 5; i++) {
                List<List<Vector3>> shuffledPhases = new ArrayList<>(originalPhases);
                Collections.shuffle(shuffledPhases);

                CompletedBalanceTestExerciseDTO test = CompletedBalanceTestExerciseDTO.builder()
                        .exercise(ExerciseEnum.Balance)
                        .difficulty(DifficultyEnum.NONE)
                        .completedAt(Instant.now().minus(4 - i, ChronoUnit.DAYS))
                        .earnedPoints(0)
                        .phase_1(shuffledPhases.get(0))
                        .phase_2(shuffledPhases.get(1))
                        .phase_3(shuffledPhases.get(2))
                        .phase_4(shuffledPhases.get(3))
                        .build();

                completedExerciseService.saveExercise(
                        test,
                        user.getId()
                );
            }
        }
    }

    private void createCompletedFireflyExercises() {
        List<User> users = List.of(
                userRepository.findByEmail("gavin@vrbalance.com").orElseThrow(() -> new NotFoundException("User not found")),
                userRepository.findByEmail("susant@vrbalance.com").orElseThrow(() -> new NotFoundException("User not found")),
                userRepository.findByEmail("ilias@vrbalance.com").orElseThrow(() -> new NotFoundException("User not found")),
                userRepository.findByEmail("patrick@vrbalance.com").orElseThrow(() -> new NotFoundException("User not found")),
                userRepository.findByEmail("amine@vrbalance.com").orElseThrow(() -> new NotFoundException("User not found")),
                userRepository.findByEmail("hoemeirra@vrbalance.com").orElseThrow(() -> new NotFoundException("User not found"))
        );

        users.forEach(this::saveFireflyExercisesForUser);
    }

    private List<CompletedFireflyExerciseDTO> generateRandomFireflyExercises() {
        return List.of(
                CompletedFireflyExerciseDTO.builder()
                        .exercise(ExerciseEnum.Firefly)
                        .difficulty(DifficultyEnum.EASY)
                        .earnedPoints(randomInt(100, 300))
                        .completedAt(Instant.now().minus(2, ChronoUnit.DAYS))
                        .caughtFirefliesCount(randomInt(0, 50))
                        .caughtWrongFirefliesCount(randomInt(10, 20))
                        .build(),

                CompletedFireflyExerciseDTO.builder()
                        .exercise(ExerciseEnum.Firefly)
                        .difficulty(DifficultyEnum.MEDIUM)
                        .earnedPoints(randomInt(100, 300))
                        .completedAt(Instant.now().minus(1, ChronoUnit.DAYS))
                        .caughtFirefliesCount(randomInt(0, 50))
                        .caughtWrongFirefliesCount(randomInt(10, 20))
                        .build(),

                CompletedFireflyExerciseDTO.builder()
                        .exercise(ExerciseEnum.Firefly)
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

        int exercisesToAdd = (int) (3 - existingCount);
        List<CompletedFireflyExerciseDTO> allExercises = generateRandomFireflyExercises();
        List<CompletedFireflyExerciseDTO> toSave = allExercises.subList(0, exercisesToAdd);

        for (CompletedFireflyExerciseDTO exercise : toSave) {
            completedExerciseService.saveExercise(
                    exercise,
                    user.getId()
            );
        }
    }

    public List<Vector3> loadPhaseData(String filename) {
        try {
            ObjectMapper mapper = new ObjectMapper();
            InputStream is = getClass().getResourceAsStream("/data/" + filename);
            if (is == null) {
                throw new FileNotFoundException("Could not find file: " + filename);
            }
            return mapper.readValue(is, new TypeReference<>() {
            });
        } catch (IOException e) {
            throw new RuntimeException("Failed to load JSON", e);
        }
    }
}
