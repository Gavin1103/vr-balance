package vr.balance.app.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.thymeleaf.spring6.SpringTemplateEngine;
import org.thymeleaf.templateresolver.ClassLoaderTemplateResolver;
import org.thymeleaf.templateresolver.ITemplateResolver;

/**
 * Configuration class for setting up Thymeleaf as the template engine.
 *
 * <p>This class defines the template resolver and the Spring-integrated
 * {@link SpringTemplateEngine}, which are used to render HTML views
 * located in the {@code templates/} directory on the classpath.
 */
@Configuration
public class ThymeleafConfig {

    /**
     * Creates and configures the {@link SpringTemplateEngine} bean.
     *
     * <p>This engine integrates Thymeleaf with Spring and uses the specified
     * {@link ITemplateResolver} to resolve HTML templates.
     *
     * @param templateResolver the template resolver used to locate HTML files
     * @return the configured {@code SpringTemplateEngine}
     */
    @Bean
    public SpringTemplateEngine springTemplateEngine(ITemplateResolver templateResolver) {
        SpringTemplateEngine templateEngine = new SpringTemplateEngine();
        templateEngine.setTemplateResolver(templateResolver);
        return templateEngine;
    }

    /**
     * Creates a {@link ClassLoaderTemplateResolver} that looks for HTML templates
     * in the {@code templates/} directory on the classpath.
     *
     * <p>The resolver is configured to:
     * <ul>
     *   <li>Look for files with the {@code .html} suffix</li>
     *   <li>Use {@code HTML} as the template mode</li>
     *   <li>Use UTF-8 character encoding</li>
     * </ul>
     *
     * @return the configured {@code ITemplateResolver}
     */
    @Bean
    public ITemplateResolver templateResolver() {
        ClassLoaderTemplateResolver resolver = new ClassLoaderTemplateResolver();
        resolver.setPrefix("templates/"); // classpath:templates/
        resolver.setSuffix(".html");
        resolver.setTemplateMode("HTML");
        resolver.setCharacterEncoding("UTF-8");
        return resolver;
    }
}