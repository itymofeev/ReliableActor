import { Component, Inject } from '@angular/core';
import { ApiService } from "../services/api.service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent {
  constructor(apiService: ApiService, router: Router, @Inject('BASE_URL') baseUrl: string) {
    this.apiService = apiService;
    this.baseUrl = baseUrl;
    this.router = router;
  }

  private router: Router;
  private apiService: ApiService;
  private baseUrl: string;

  public expression: string = '';
  public isValid: boolean = true;
  public extractionInProgress: boolean = false;

  public extractVariables() {
    this.isValid = this.expression.trim() !== '';
    if (!this.isValid) {
      return;
    }
    this.extractionInProgress = true;
    this.apiService.startVariableExtraction(this.expression, 'http://localhost:8663').then(result => {
      this.extractionInProgress = false;
      if (!result.variables.length) {
        console.log('Empty result');
        return;
      }
      this.router.navigate(['/variable-editor'], { queryParams: { variables: JSON.stringify(result.variables), expression: this.expression } });
    }).catch(() => {
      this.extractionInProgress = false;
    });
  }
}
