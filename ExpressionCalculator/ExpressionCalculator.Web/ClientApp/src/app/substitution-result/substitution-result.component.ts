import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: 'app-substitution-result',
  templateUrl: './substitution-result.component.html'
})
export class SubstitutionResultComponent {
  public substituteExpression: string;

  constructor(activatedRoute: ActivatedRoute, router: Router) {
    this.substituteExpression = activatedRoute.snapshot.queryParamMap.get('substituteExpression');
    if (this.substituteExpression === '') {
      router.navigate(['/']);
    }
  }
}
