import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: 'app-variable-editor',
  templateUrl: './variable-editor.component.html'
})
export class VariableEditor {
  constructor(activatedRoute: ActivatedRoute, router: Router) {
    let variablesStr = activatedRoute.snapshot.queryParamMap.get('variables');
    let expression = activatedRoute.snapshot.queryParamMap.get('expression');
    if (variablesStr === '' || expression == '') {
      router.navigate(['/'])
    }

    this.variables = <string[]><any>variablesStr;
    this.expression = expression;
  }

  private variables: string[];
  private expression: string;
}
