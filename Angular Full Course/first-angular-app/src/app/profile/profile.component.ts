import { Component, Input, OnChanges, SimpleChanges, OnInit, DoCheck, AfterContentInit, AfterContentChecked, AfterViewInit, AfterViewChecked, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnChanges, OnInit, DoCheck, AfterContentInit, AfterContentChecked, AfterViewInit, AfterViewChecked, OnDestroy {

  constructor() {
    console.log("profile constructor triggered");
    console.log(this.pUserName);
  }

  @Input() pUserName : string = "";

  counter: number = 0;

  incrementCounter() {
    this.counter++;
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log("ngOnChanges triggered");
    console.log(this.pUserName);
  }

  ngOnInit(): void {
    console.log("ngOnInit triggered");
  }

  ngDoCheck(): void {
    console.log("ngDoCheck triggered");
  }

  ngAfterContentInit(): void {
    console.log("ngAfterContentInit triggered")
  }

  ngAfterContentChecked(): void {
    console.log("ngAfterContentChecked triggered")
  }

  ngAfterViewInit(): void {
    console.log("ngAfterViewInit triggered")
  }

  ngAfterViewChecked(): void {
    console.log("ngAfterViewChecked triggered")
  }

  ngOnDestroy(): void {
    console.log("ngOnDestroy triggered")
  }
}
