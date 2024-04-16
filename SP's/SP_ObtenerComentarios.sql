USE [MinimalAPIsCurso]
GO
/****** Object:  StoredProcedure [dbo].[SP_ObtenerComentarios]    Script Date: 15/04/2024 10:05:35 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_ObtenerComentarios]
	@PeliculaId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Comentarios WHERE PeliculaId = @PeliculaId;
END
