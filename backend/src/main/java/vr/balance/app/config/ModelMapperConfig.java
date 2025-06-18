package vr.balance.app.config;

import org.modelmapper.ModelMapper;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

/**
 * Configuration class for setting up {@link ModelMapper} as a Spring Bean.
 *
 * <p>{@code ModelMapper} is a library used to automatically map between DTOs and entities,
 * reducing boilerplate code in service and controller layers.
 *
 * <p>The bean can be injected wherever object mapping is needed throughout the application.
 *
 */
@Configuration
public class ModelMapperConfig {

    /**
     * Registers a {@link ModelMapper} bean in the Spring application context.
     *
     * @return a new instance of {@code ModelMapper}
     */
    @Bean
    public ModelMapper modelMapper() {
        return new ModelMapper();
    }
}