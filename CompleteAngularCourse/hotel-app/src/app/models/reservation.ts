//created  with
// >ng g i models/reservation

export interface Reservation {
    id:string,
    checkInDate: Date,
    checkOutDate: Date,
    guestName: string,
    guestEmail: string,
    roomNumber: number
}
