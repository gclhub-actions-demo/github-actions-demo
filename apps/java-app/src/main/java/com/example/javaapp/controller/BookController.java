package com.example.javaapp.controller;

import com.example.javaapp.model.Book;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.atomic.AtomicLong;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

/** REST controller for book management. */
@RestController
@RequestMapping("/api/books")
public class BookController {

  private final Map<Long, Book> books = new HashMap<>();
  private final AtomicLong idCounter = new AtomicLong(1);

  /** Constructor initializes sample data. */
  public BookController() {
    books.put(1L, new Book(1L, "The Great Gatsby", "F. Scott Fitzgerald", 1925));
    books.put(2L, new Book(2L, "1984", "George Orwell", 1949));
    books.put(3L, new Book(3L, "To Kill a Mockingbird", "Harper Lee", 1960));
    idCounter.set(4);
  }

  /**
   * Get all books.
   *
   * @return list of all books
   */
  @GetMapping
  public ResponseEntity<Map<String, List<Book>>> getAllBooks() {
    Map<String, List<Book>> response = new HashMap<>();
    response.put("books", new ArrayList<>(books.values()));
    return ResponseEntity.ok(response);
  }

  /**
   * Get a book by ID.
   *
   * @param id book ID
   * @return book if found, 404 otherwise
   */
  @GetMapping("/{id}")
  public ResponseEntity<Book> getBook(@PathVariable Long id) {
    Book book = books.get(id);
    if (book != null) {
      return ResponseEntity.ok(book);
    }
    return ResponseEntity.notFound().build();
  }

  /**
   * Create a new book.
   *
   * @param book book to create
   * @return created book
   */
  @PostMapping
  public ResponseEntity<Book> createBook(@RequestBody Book book) {
    if (book.getTitle() == null || book.getAuthor() == null) {
      return ResponseEntity.badRequest().build();
    }

    long id = idCounter.getAndIncrement();
    Book newBook = new Book(id, book.getTitle(), book.getAuthor(), book.getYear());
    books.put(id, newBook);
    return ResponseEntity.status(HttpStatus.CREATED).body(newBook);
  }

  /**
   * Delete a book by ID.
   *
   * @param id book ID
   * @return 204 if deleted, 404 if not found
   */
  @DeleteMapping("/{id}")
  public ResponseEntity<Void> deleteBook(@PathVariable Long id) {
    if (books.remove(id) != null) {
      return ResponseEntity.noContent().build();
    }
    return ResponseEntity.notFound().build();
  }
}
