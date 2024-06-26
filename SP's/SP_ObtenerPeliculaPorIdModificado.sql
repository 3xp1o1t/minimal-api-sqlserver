USE [MinimalAPIsCurso]
GO
/****** Object:  StoredProcedure [dbo].[SP_ObtenerPeliculaPorId]    Script Date: 28/04/2024 04:28:43 p. m. ******/
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
	SELECT Id, Nombre FROM Generos
	INNER JOIN GenerosPeliculas
	ON GenerosPeliculas.GeneroId = Id
	WHERE PeliculaId = @Id;
	SELECT Id, Nombre, Personaje
	FROM Actores
	INNER JOIN ActoresPeliculas
	ON ActoresPeliculas.ActorId = Id
	WHERE PeliculaId = @Id
	ORDER BY Orden
END
