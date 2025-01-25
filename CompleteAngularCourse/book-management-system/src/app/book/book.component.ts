import { Component, OnInit } from '@angular/core';
import { Book } from '../models/book.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-book',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './book.component.html',
  styleUrl: './book.component.css'
})
export class BookComponent implements OnInit{
  books: Book[] = []

  newTitle: string = ""
  newAuthor: string = ""

  ngOnInit(): void {
    this.getBooks()
  }

  addBook() {
    if (this.newTitle && this.newAuthor) {
      let newBook: Book = {
        id: Date.now(),
        title: this.newTitle,
        author: this.newAuthor
      }

      this.books.push(newBook)

      localStorage.setItem("books", JSON.stringify(this.books))
    }
  }

  getBooks() {
    var savedBooks = localStorage.getItem("books")
    if (savedBooks) {
      this.books = JSON.parse(savedBooks)
    }
  }

  deleteBook(index:number) {
    this.books.splice(index, 1)

    localStorage.setItem("books", JSON.stringify(this.books))
  }
}
