package vr.balance.app.util;

import java.security.SecureRandom;

public class PincodeGenerator {

    private static final SecureRandom random = new SecureRandom();

    public static String generatePincode() {
        int pincode = random.nextInt(9000) + 1000; // Number between 1000 and 9999
        return String.valueOf(pincode);
    }
}