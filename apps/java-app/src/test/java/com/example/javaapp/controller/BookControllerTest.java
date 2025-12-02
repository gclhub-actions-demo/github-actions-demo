package com.example.javaapp.controller;

import com.example.javaapp.model.Book;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;

import static org.hamcrest.Matchers.hasSize;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.delete;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

/**
 * Test class for BookController.
 */
@WebMvcTest(BookController.class)
class BookControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @Test
    void getAllBooks_ShouldReturnBooks() throws Exception {
        mockMvc.perform(get("/api/books"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.books").isArray())
                .andExpect(jsonPath("$.books", hasSize(3)));
    }

    @Test
    void getBook_WhenExists_ShouldReturnBook() throws Exception {
        mockMvc.perform(get("/api/books/1"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.id").value(1))
                .andExpect(jsonPath("$.title").value("The Great Gatsby"))
                .andExpect(jsonPath("$.author").value("F. Scott Fitzgerald"));
    }

    @Test
    void getBook_WhenNotExists_ShouldReturn404() throws Exception {
        mockMvc.perform(get("/api/books/999"))
                .andExpect(status().isNotFound());
    }

    @Test
    void createBook_WithValidData_ShouldReturnCreated() throws Exception {
        Book newBook = new Book(null, "New Book", "New Author", 2024);

        mockMvc.perform(post("/api/books")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(newBook)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.id").exists())
                .andExpect(jsonPath("$.title").value("New Book"))
                .andExpect(jsonPath("$.author").value("New Author"))
                .andExpect(jsonPath("$.year").value(2024));
    }

    @Test
    void createBook_WithMissingTitle_ShouldReturnBadRequest() throws Exception {
        Book invalidBook = new Book(null, null, "Author", 2024);

        mockMvc.perform(post("/api/books")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(invalidBook)))
                .andExpect(status().isBadRequest());
    }

    @Test
    void createBook_WithMissingAuthor_ShouldReturnBadRequest() throws Exception {
        Book invalidBook = new Book(null, "Title", null, 2024);

        mockMvc.perform(post("/api/books")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(invalidBook)))
                .andExpect(status().isBadRequest());
    }

    @Test
    void deleteBook_WhenExists_ShouldReturn204() throws Exception {
        mockMvc.perform(delete("/api/books/1"))
                .andExpect(status().isNoContent());
    }

    @Test
    void deleteBook_WhenNotExists_ShouldReturn404() throws Exception {
        mockMvc.perform(delete("/api/books/999"))
                .andExpect(status().isNotFound());
    }
}
