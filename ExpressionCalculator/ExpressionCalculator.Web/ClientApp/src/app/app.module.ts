import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { VariableEditorComponent } from './variable-editor/variable-editor.component';
import { SubstitutionResultComponent } from './substitution-result/substitution-result.component'
import { ApiService } from "./services/api.service";

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    VariableEditorComponent,
    SubstitutionResultComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'variable-editor', component: VariableEditorComponent },
      { path: 'substitution-result', component: SubstitutionResultComponent },
    ])
  ],
  providers: [ApiService],
  bootstrap: [AppComponent]
})
export class AppModule { }
