﻿CREATE PROC FI_SP_ListBeneficiario
	@IDCLIENTE BIGINT
AS
BEGIN
	SELECT Id, IDCLIENTE, CPF, NOME FROM BENEFICIARIOS WITH(NOLOCK) WHERE IDCLIENTE = @IDCLIENTE
END