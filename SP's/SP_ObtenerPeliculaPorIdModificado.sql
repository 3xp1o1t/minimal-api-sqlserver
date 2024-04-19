USE [MinimalAPIsCurso]
GO
/****** Object:  StoredProcedure [dbo].[SP_ObtenerPeliculaPorId]    Script Date: 18/04/2024 11:08:02 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_ObtenerPeliculaPorId]
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Peliculas WHERE Id = @Id;
	SELECT * FROM Comentarios WHERE PeliculaId = @Id;
END
