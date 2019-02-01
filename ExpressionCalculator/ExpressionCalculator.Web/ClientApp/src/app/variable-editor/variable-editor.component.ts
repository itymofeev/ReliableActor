import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { ApiService } from "../services/api.service";

@Component({
  selector: 'app-variable-editor',
  templateUrl: './variable-editor.component.html'
})
export class VariableEditorComponent {
  constructor(activatedRoute: ActivatedRoute, router: Router, apiService: ApiService, @Inject('BASE_URL') baseUrl: string) {
    let variablesStr = activatedRoute.snapshot.queryParamMap.get('variables');
    let expression = activatedRoute.snapshot.queryParamMap.get('expression');
    if (variablesStr === '' || expression === '') {
      router.navigate(['/']);
    }

    this.expression = expression;
    this.variables = (JSON.parse(variablesStr) as string[]).map<ViewModels.IVariableToValueEntry>(entry => ({ name: entry, value: '' }) as ViewModels.IVariableToValueEntry);
    this.baseUrl = baseUrl;
    this.apiService = apiService;
    this.router = router;
  }

  public variables: ViewModels.IVariableToValueEntry[];
  public expression: string;

  private baseUrl: string;
  private apiService: ApiService;
  private router: Router;

  public replaceVariables() {
    this.apiService.substituteVariable(this.variables, this.expression, 'http://localhost:8663').then(result => {
      this.router.navigate(['/substitution-result'], { queryParams: { substituteExpression: result } });
    }).catch(() => {
      console.log('An Error has occured!!!');
    });
  }
}
