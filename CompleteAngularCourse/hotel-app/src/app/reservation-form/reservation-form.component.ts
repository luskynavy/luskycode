//created and linked to module home with
// >ng g reservation-form home --module=reservation

import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-reservation-form',
  templateUrl: './reservation-form.component.html',
  styleUrl: './reservation-form.component.css'
})
export class ReservationFormComponent {
  reservationForm: FormGroup = new FormGroup({});

  onSubmit() {
    
  }
}
