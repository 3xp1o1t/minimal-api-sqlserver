USE [MinimalAPIsCurso]
GO
/****** Object:  StoredProcedure [dbo].[SP_ObtenerActores]    Script Date: 09/04/2024 09:06:30 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_ObtenerActores] 
	@pagina int,
	@registrosPorPagina int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Actores ORDER BY Nombre
	OFFSET ((@pagina - 1) * @registrosPorPagina) ROWS FETCH NEXT @registrosPorPagina ROWS ONLY
END
