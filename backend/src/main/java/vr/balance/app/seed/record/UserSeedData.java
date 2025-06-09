package vr.balance.app.seed.record;

import vr.balance.app.enums.RoleEnum;

public record UserSeedData(
        String email,
        String username,
        String password,
        String pincode,
        RoleEnum role
) {}