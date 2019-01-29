import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/interval';
import { timer } from 'rxjs/observable/timer';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent {
  public expression: string;
  public isValid: boolean;
  public correlationId: string;

  private http: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.isValid = true;
  }

  public extractVariables() {
    this.isValid = this.expression.trim() !== '';
    if (!this.isValid) {
      return;
    }
    this.startVariableExtraction().subscribe(this.tryGetExtractedVariables, this.handleError);
  }

  private startVariableExtraction() {
    return this.http.post<string>('http://localhost:8663/api/extractvariable', this.expression);
  }

  private tryGetExtractedVariables(correlationId: string) {
    const delay = 1000; // every 1 sec
    const count = 5; // process it 5 times

   // Observable.interval(delay).take(count).subscribe(() => {
   //   this.http.get<string>('http://localhost:8663/api/extractvariable' + correlationId)
   // });

    //const subscription = timer(0, 1000).subscribe(() => );
  }

  private handleError(error: HttpErrorResponse) {
    console.error(error); // Some error handling here
  }
}
