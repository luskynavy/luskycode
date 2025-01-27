//created  with
// >ng g service reservation/reservation

import { Injectable } from '@angular/core';
import { Reservation } from '../models/reservation';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {

  private reservations: Reservation[] = []

  constructor() {
    let savedReservations = localStorage.getItem("reservations")
    if (savedReservations) {
      this.reservations = JSON.parse(savedReservations);
    }
  }

  getReservations(): Reservation[] {
    return this.reservations;
  }

  getReservation(id: string): Reservation | undefined {
    return this.reservations.find(res => res.id == id);
  }

  addReservation(reservation: Reservation): void {
    reservation.id = Date.now().toString();

    this.reservations.push(reservation)

    localStorage.setItem("reservations", JSON.stringify(this.reservations));
  }

  deleteReservation(id: string): void {
    let index = this.reservations.findIndex(res => res.id == id);
    this.reservations.splice(index, 1);

    localStorage.setItem("reservations", JSON.stringify(this.reservations));
  }

  updateReservation(id: string, reservation: Reservation): void {
    let index = this.reservations.findIndex(res => res.id == id);;
    reservation.id = id;
    this.reservations[index] = reservation;

    localStorage.setItem("reservations", JSON.stringify(this.reservations));
  }
}
