package vr.balance.app.util;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.Map;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;

/**
 * Utility class for creating ZIP archives from string content.
 */
public class Zip {

    /**
     * Creates a ZIP archive from a map where each entry represents a filename and its corresponding content.
     *
     * <p>Each entry will be added as a separate file in the specified folder within the archive.
     *
     * @param folder the folder path to place all files in (e.g., "results/"). Must end with "/" if you want it treated as a folder
     * @param files  a map containing filename as the key and file content as the value
     * @return a byte array representing the ZIP file
     * @throws IOException if an I/O error occurs while writing the ZIP content
     */
    public static byte[] createZipFromMap(String folder, Map<String, String> files) throws IOException {
        ByteArrayOutputStream baos = new ByteArrayOutputStream();
        try (ZipOutputStream zos = new ZipOutputStream(baos)) {
            for (Map.Entry<String, String> entry : files.entrySet()) {
                ZipEntry zipEntry = new ZipEntry(folder + entry.getKey());
                zos.putNextEntry(zipEntry);
                zos.write(entry.getValue().getBytes(StandardCharsets.UTF_8));
                zos.closeEntry();
            }
        }
        return baos.toByteArray();
    }
}