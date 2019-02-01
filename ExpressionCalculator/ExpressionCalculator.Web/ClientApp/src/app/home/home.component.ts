import { Component } from '@angular/core';
import { ApiService } from "../services/api.service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  constructor(apiService: ApiService, router: Router) {
    this.apiService = apiService;
    this.router = router;
  }

  private router: Router;
  private apiService: ApiService;

  public expression: string = '';
  public isValid: boolean = true;
  public extractionInProgress: boolean = false;

  public extractVariables() {
    this.isValid = this.expression.trim() !== '';
    if (!this.isValid) {
      return;
    }
    this.extractionInProgress = true;
    this.apiService.startVariableExtraction(this.expression).then(result => {
      this.extractionInProgress = false;
      if (!result.variables.length) {
        this.router.navigate(['/substitution-result'], { queryParams: { substituteExpression: this.expression } });
        return;
      }
      this.router.navigate(['/variable-editor'], { queryParams: { variables: JSON.stringify(result.variables), expression: this.expression } });
    }).catch(() => {
      this.extractionInProgress = false;
    });
  }
}
