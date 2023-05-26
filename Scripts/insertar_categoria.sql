DROP FUNCTION insertar_categoria(smallint,integer,character varying);

CREATE OR REPLACE FUNCTION insertar_categoria (opcion SMALLINT, num_categoria INTEGER, nom_categoria VARCHAR(50))
RETURNS VOID AS
$$
	BEGIN
	
		IF opcion = 0 THEN
			INSERT INTO tb_categorias(numero_categoria, nombre_categoria)
			VALUES (num_categoria, nom_categoria);
		ELSIF opcion = 1 THEN
			UPDATE tb_categorias
			SET nombre_categoria = nom_categoria
			WHERE numero_categoria = num_categoria;
		ELSIF opcion = 2 THEN
			UPDATE tb_categorias
			SET estado = 1
			WHERE numero_categoria = num_categoria;
		END IF;
		
	END
$$
LANGUAGE 'plpgsql';