# Mecânica

## Elementos Formais do Jogo


**Defense of Castle** é um jogo de estratégia do gênero *tower defense*, onde o jogador deve defender seu reino contra ondas de inimigos, construindo e posicionando torres de defesa ao longo do mapa. A mecânica do jogo gira em torno do posicionamento estratégico das torres e do gerenciamento das moedas .



## Padrão de Interação do Jogador

O jogador interage com o jogo por meio de cliques na tela, podendo:

- Chamar a próxima onda de inimigos.
- Iniciar a fase.
- Pular ou avançar as cutscenes.
- Selecionar slot e escolher a torre que deseja construir
  
## Objetivo do Jogo

O objetivo principal do jogo é impedir que as ondas de inimigos alcancem o final do caminho, defendendo pontos estratégicos e protegendo o reino.

- **Quando o jogador ganha?**  
  O jogador vence quando consegue defender todas as ondas de inimigos em uma fase sem que o inimigo atinja o ponto final do mapa. Para isso, o jogador deve derrotar todos os inimigos sem que o número de vidas seja reduzido a zero.

- **Quando o jogador perde?**  
  O jogador perde quando o inimigo chega ao ponto final do mapa, resultando na perda de uma quantidade significativa de vidas. Se as vidas se esgotarem, a fase termina e é exibido a tela de derrota.



## Regras do Jogo

1. **Construção de Torres:** O jogador pode construir torres apenas em locais pré-definidos no mapa.
2. **Melhoria de Torres:** As torres podem ser melhoradas para aumentar seu poder de ataque, alcance e outras características, tais melhorias custam ouro.
3. **Tipos de Torres:** Existem diferentes tipos de torres, como arquerias, canhões, magos e torres de defesa especial, cada uma com características próprias.
4. **Inimigos:** As ondas de inimigos têm diferentes características e fraquezas, como resistência a certos tipos de dano ou a habilidade de se mover rapidamente.
5. **Recursos:** O jogo gira em torno de ouro, moeda ultilizada para contruir mais defesas, ele possue uma quantidade finita que da para adquirir por fase.
6. **Vidas:** O jogador começa com um número determinado de vidas, e cada inimigo que chega ao final do caminho reduz o total de vidas.



## Procedimentos do Jogo

O jogo é dividido em fases, cada uma com um mapa específico e uma trilha que os inimigos seguem. As fases funcionam da seguinte maneira:

- **Início da Fase:** Cada fase começa com uma onda de inimigos, e o jogador deve defender o caminho utilizando as torres.
- **Ganho de Ouro:** Ao derrotar inimigos, o jogador ganha ouro, que é utilizado para construir novas torres ou melhorar as existentes.



## Recursos do Jogo

1. **Ouro:** Usado para construir e melhorar torres.
2. **Vidas:** Indicam o número de ataques que o objetivo pode sofrer antes de ser derrubado (derrota). O jogador começa com um número de vidas fixo de 25, e cada inimigo que atinge o final do caminho causa a perda de vidas.


## Limites do Jogo

1. **Limite de Construção de Torres:** O jogador só pode construir torres em locais específicos do mapa.
2. **Limite de Recursos:** O jogador deve gerenciar o ouro, pois não é possível construir ou melhorar torres caso não haja ouro suficiente, cada fase o jogador começa com uma certa quantidade de ouro .
3. **Limite de Vidas:** O número de vidas é finito, e uma vez que todas as vidas forem perdidas, o jogo termina.
4. **Dificuldade Crescente:** A cada nova fase, as ondas de inimigos ficam mais difíceis, exigindo mais estratégia do jogador.


## Resultados do Jogo

- **Como ele termina depois da vitória?**  
  Quando o jogador derrota o ultimo minion da ultima wave da fase, o jogo exibe uma tela de vitória, desbloqueia e mostra uma cutscene e, em seguida, a próxima fase é liberada.

- **Como ele termina depois da derrota?**  
  Conforme os inimigos chegam ao seu destino vivos, o contador de vidas vai diminuindo, quando chega a 0 a fase termina com uma tela de derrota. O jogador pode optar por tentar novamente a fase ou retornar ao menu principal.
> Nota: Caso a fase seja a última e a mesma foi perdida, tocará uma cutscene alternativa e a fase não é concluida.

