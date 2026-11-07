-- =============================================================================
-- RetroGames - Script 04: testes de integridade
-- Referência: Espec. 4.3
--
-- RODE UM COMANDO POR VEZ (cursor no comando + Ctrl+Enter).
-- Os testes 1 a 4 DEVEM dar erro: é isso que prova que o banco protege a
-- estrutura. O teste 5 deve ser aceito, porque a regra "preço > 0" fica no
-- Service (RN06), não no banco.
-- =============================================================================

USE retrogames_beatriz;

-- -----------------------------------------------------------------------------
-- Teste 1: inserir jogo com co_plataforma inexistente
-- Esperado: Erro 1452 (chave estrangeira)
-- Resultado obtido: (preencher)
-- -----------------------------------------------------------------------------
INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES ('Teste plataforma inexistente', NULL, 1990, 50.00, 1, TRUE, UTC_TIMESTAMP(), 999, 2);

-- -----------------------------------------------------------------------------
-- Teste 2: inserir plataforma com nome já cadastrado
-- Esperado: Erro 1062 (UNIQUE)
-- Resultado obtido: (preencher)
-- -----------------------------------------------------------------------------
INSERT INTO tb_plataforma (no_plataforma, no_fabricante, nu_ano_lancamento)
VALUES ('NES', 'Nintendo', 1983);

-- -----------------------------------------------------------------------------
-- Teste 3: excluir plataforma que possui jogos (1 = Atari 2600)
-- Esperado: Erro 1451 (chave estrangeira)
-- Resultado obtido: (preencher)
-- -----------------------------------------------------------------------------
DELETE FROM tb_plataforma WHERE co_seq_plataforma = 1;

-- -----------------------------------------------------------------------------
-- Teste 4: inserir jogo com no_jogo nulo
-- Esperado: Erro 1048 (NOT NULL)
-- Resultado obtido: (preencher)
-- -----------------------------------------------------------------------------
INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES (NULL, NULL, 1990, 50.00, 1, TRUE, UTC_TIMESTAMP(), 1, 2);

-- -----------------------------------------------------------------------------
-- Teste 5: inserir jogo com vl_preco igual a 0
-- Esperado: Aceito (1 row affected): a regra fica no Service (RN06)
-- Resultado obtido: (preencher)
-- -----------------------------------------------------------------------------
INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES ('Teste preço zero', NULL, 1990, 0.00, 1, TRUE, UTC_TIMESTAMP(), 1, 2);

-- -----------------------------------------------------------------------------
-- Limpeza: remove o jogo inserido pelo teste 5.
-- Os dados de exemplo vão até o id 22, então tudo acima disso é dado de teste.
-- O filtro usa a chave primária, o que o "safe update mode" do Workbench exige.
-- -----------------------------------------------------------------------------
DELETE FROM tb_jogo WHERE co_seq_jogo > 22;

-- Conferência final. Esperado: 22 jogos, 8 plataformas e nenhum jogo de teste
SELECT (SELECT COUNT(*) FROM tb_jogo)       AS jogos,
       (SELECT COUNT(*) FROM tb_plataforma) AS plataformas,
       (SELECT COUNT(*) FROM tb_jogo WHERE no_jogo LIKE 'Teste%') AS jogos_de_teste;
