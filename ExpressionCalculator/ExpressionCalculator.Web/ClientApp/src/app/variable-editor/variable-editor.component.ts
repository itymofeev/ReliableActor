import { Component } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { ApiService } from "../services/api.service";

@Component({
  selector: 'app-variable-editor',
  templateUrl: './variable-editor.component.html',
  styleUrls: ['./variable-editor.component.css']
})
export class VariableEditorComponent {
  constructor(activatedRoute: ActivatedRoute, router: Router, apiService: ApiService) {
    let variablesStr = activatedRoute.snapshot.queryParamMap.get('variables');
    let expression = activatedRoute.snapshot.queryParamMap.get('expression');
    if (variablesStr === '' || expression === '') {
      router.navigate(['/']);
    }

    this.expression = expression;
    this.variables = (JSON.parse(variablesStr) as string[]).map<ViewModels.IVariableToValueEntry>(entry => ({ name: entry, value: '' }) as ViewModels.IVariableToValueEntry);
    this.apiService = apiService;
    this.router = router;
  }

  public variables: ViewModels.IVariableToValueEntry[];
  public expression: string;

  private apiService: ApiService;
  private router: Router;

  public replaceVariables() {
    this.apiService.substituteVariable(this.variables, this.expression).then(result => {
      this.router.navigate(['/substitution-result'], { queryParams: { substituteExpression: result } });
    }).catch(() => {
      console.log('An Error has occured!!!');
    });
  }
}
