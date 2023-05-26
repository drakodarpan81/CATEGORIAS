CREATE OR REPLACE FUNCTION consultar_categoria(categoria SMALLINT, OUT nombre VARCHAR(50))
RETURNS VARCHAR(50) AS 
$$

	BEGIN
	
		nombre := (SELECT nombre_categoria
		FROM tb_categorias
		WHERE numero_categoria = categoria);
	
	END

$$
LANGUAGE 'plpgsql';