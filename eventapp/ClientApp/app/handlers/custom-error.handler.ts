import { Injectable, ErrorHandler } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class CustomErrorHandler  implements ErrorHandler {

  constructor(private snackBar: MatSnackBar) { }

  handleError(error) {
    console.log(error);
    this.snackBar.open('Oh no! Something happened. Please try again later', 'Dismiss', {
      duration: 7000
    });
  }
}
