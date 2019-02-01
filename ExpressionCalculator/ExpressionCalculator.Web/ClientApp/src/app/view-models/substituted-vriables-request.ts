namespace ViewModels {
  export interface ISubstitutedVariablesRequest {
    variablesToValuesMap: IVariableToValueEntry[];
    expression: string;
  }
}
