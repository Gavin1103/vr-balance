package vr.balance.app.seed.record;

public record UserStatsSeedData(
        String email,
        int totalPoints,
        int currentStreak,
        int highestStreak
) {}