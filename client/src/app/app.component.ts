import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http'
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  users: any

  constructor(private readonly httpClient: HttpClient) {
  }

  ngOnInit() {
    this.httpClient.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: error => {console.log(error)},
      complete: () => {console.log('request completed')}
    })
  }
}
