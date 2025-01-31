import { Injectable } from '@angular/core';
import { Book } from '../models/book';
import { Observable, of, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  constructor() { }

  addBook(book: Book): Observable<Book> {
    if (book.id && book.title && book.title)
    {
      return of(book);
    } else {
      const err = new Error('Id, title and author must not be empty')
      return throwError(() => err)
    }
  }
}
