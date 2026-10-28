package com.example.javaapp;

import static org.mockito.Mockito.mockStatic;

import org.junit.jupiter.api.Test;
import org.mockito.MockedStatic;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.test.context.SpringBootTest;

/**
 * Test class for main application.
 */
@SpringBootTest
class JavaAppApplicationTests {

    @Test
    void contextLoads() {
        // Test that Spring context loads successfully
    }

    @Test
    void mainForwardsArgumentsToSpringBoot() {
        String[] args = {"--server.port=0"};
        try (MockedStatic<SpringApplication> application = mockStatic(SpringApplication.class)) {
            JavaAppApplication.main(args);
            application.verify(() -> SpringApplication.run(JavaAppApplication.class, args));
        }
    }
}
