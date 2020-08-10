CxList outputs = Find_Ktor_Outputs();
outputs.Add(Find_Vertx_Outputs());
outputs -= Find_LambdaExpr();
result.Add(outputs);