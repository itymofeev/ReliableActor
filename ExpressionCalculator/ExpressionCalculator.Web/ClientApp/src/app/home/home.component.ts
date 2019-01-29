import { Component, Inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiService } from "../services/api.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent {
  public expression: string = '';
  public isValid: boolean = true;
  public extractionInProgress: boolean = false;

  private apiService: ApiService;
  private baseUrl: string;

  constructor(apiService: ApiService, @Inject('BASE_URL') baseUrl: string) {
    this.apiService = apiService;
    this.baseUrl = baseUrl;
  }

  public extractVariables() {
    this.isValid = this.expression.trim() !== '';
    if (!this.isValid) {
      return;
    }
    this.extractionInProgress = true;
    var res = this.apiService.startVariableExtraction(this.expression, 'http://localhost:8663');
    res.then(x => {
      this.extractionInProgress = false;
      if (!x.variables.length) {
        console.log('Empty result');
      }
    }).catch(() => {
      this.extractionInProgress = false;
    });
  }

  private onProcessingFinish(result: ViewModels.IExtractedVariables) {
    this.extractionInProgress = false;
    if (!result.variables.length) {
      console.log('Empty result');
    }
  }

  private onError(error: HttpErrorResponse) {
    
  }
}
