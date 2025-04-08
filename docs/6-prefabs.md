### 1. Prefabs: Torres Elementais (Fogo, Água, Terra)

*   **Nomes:**
    *   `Prefab_Torre_Fogo`
    *   `Prefab_Torre_Agua`
    *   `Prefab_Torre_Terra`
*   **Descrição:** Representam as estruturas de defesa que o jogador constrói. Cada torre pertence a um elemento (Fogo, Água ou Terra) e ataca inimigos que passam por perto. A ideia é que o dano possa variar dependendo da combinação elemento da torre vs tipo de inimigo.
*   **Quando são utilizados:** Instanciados (criados no jogo) quando o jogador seleciona um local válido no mapa e escolhe construir uma torre daquele elemento específico.
*   **Quais seus componentes:**
    *   **Sprites:** Imagem visual da torre correspondente.
    *   **Colisores:** Para detectar cliques do jogador (seleção, interação) e definir a área da torre.
    *   **Fontes de áudio:** Para sons de ataque, construção, etc.
    *   **Scripts:** Para controlar toda a lógica da torre.
*   **Descreva o comportamento dos scripts:**
    *   O script principal da torre procura por inimigos dentro de um raio de alcance definido.
    *   Ao encontrar um alvo, comanda a torre para realizar um ataque (geralmente instanciando um projétil).
    *   Implementa um tempo de espera (cooldown) entre os ataques.
    *   Pode gerenciar dados como custo, dano, alcance e velocidade de ataque.

---

### 2. Prefab: Inimigo Base

*   **Nome:** `Prefab_Inimigo_Base`
*   **Descrição:** Modelo fundamental para as unidades inimigas que avançam pelo cenário. No jogo final, teremos variações (Orcs, Goblins, etc.), mas todos partem deste conceito base: mover-se pelo caminho e ter pontos de vida.
*   **Quando são utilizados:** Instanciados pelo sistema de "ondas" (waves) do jogo, aparecendo no ponto inicial do caminho definido no mapa.
*   **Quais seus componentes:**
    *   **Sprites:** Aparência visual do inimigo. Pode ter animações básicas (andar).
    *   **Colisores:** Para ser detectado pelas torres e atingido por projéteis.
    *   **Fontes de áudio:** Para sons ao ser atingido, ao morrer, etc.
    *   **Scripts:** Para controlar movimento e estado do inimigo.
    *   *(Componente Técnico Frequente):* `Rigidbody2D` (muitas vezes `Is Kinematic`) para funcionar corretamente com colisões de projéteis baseadas em trigger.
*   **Descreva o comportamento dos scripts:**
    *   Um script controla o movimento do inimigo, fazendo-o seguir uma sequência de pontos (waypoints) que definem o caminho.
    *   Outro script gerencia a saúde (`vida`) do inimigo, reduzindo-a quando recebe dano.
    *   Quando a vida chega a zero, o script de saúde aciona a lógica de morte (remove o inimigo, pode dar recompensa ao jogador).

---

### 3. Prefab: Chefe Base

*   **Nome:** `Prefab_Chefe_Base`
*   **Descrição:** Representa um inimigo especial, muito mais forte e resistente que os inimigos comuns. Aparece em momentos chave do jogo como um desafio maior. Geralmente possui um visual distinto e mais vida.
*   **Quando são utilizados:** Instanciado em momentos específicos, como o final de um conjunto de ondas ou em fases designadas como "Boss Stage".
*   **Quais seus componentes:**
    *   **Sprites:** Visual maior e/ou mais detalhado que inimigos normais.
    *   **Colisores:** Para detecção e acerto, potencialmente maior.
    *   **Fontes de áudio:** Sons específicos para o chefe (ataque, dano, morte).
    *   **Scripts:** Para controlar seu comportamento (que pode ser mais complexo).
    *   *(Componente Técnico Frequente):* `Rigidbody2D`.
*   **Descreva o comportamento dos scripts:**
    *   Similar ao inimigo base no movimento (segue o caminho), mas gerencia uma quantidade muito maior de vida.
    *   Pode incluir lógicas para resistências ou habilidades especiais (conceito).
    *   O script de morte geralmente concede uma recompensa maior e pode sinalizar um avanço importante no jogo.

---

### 4. Prefab: Projétil

*   **Nome:** `Prefab_Projetil_Base` (ou específicos como `_Fogo`, `_Agua`, `_Terra`)
*   **Descrição:** O objeto que é disparado pela torre e viaja em direção ao inimigo para causar dano (ex: bola de fogo, flecha, raio).
*   **Quando são utilizados:** Instanciado pela script da Torre no momento do ataque.
*   **Quais seus componentes:**
    *   **Sprites:** A imagem visual do projétil.
    *   **Colisores:** Configurado como `Is Trigger` para detectar a colisão com o inimigo sem empurrá-lo fisicamente.
    *   **Fontes de áudio:** Opcional, para som de impacto (pode ser tocado no inimigo também).
    *   **Scripts:** Para controlar o movimento e a lógica de colisão.
    *   *(Componente Técnico Opcional):* `Rigidbody2D` se usar física para movimento ou detecção.
*   **Descreva o comportamento dos scripts:**
    *   Controla o movimento do projétil (em linha reta ou seguindo o alvo).
    *   Detecta colisões usando `OnTriggerEnter2D`.
    *   Ao colidir com um objeto com a tag "Inimigo", chama a função de receber dano no script do inimigo.
    *   Após a colisão (ou após um tempo/distância máxima), o script destrói o próprio objeto projétil.

---

### 5. Prefab: Efeito Visual (VFX)

*   **Nome:** `Prefab_Efeito_Impacto`, `Prefab_Efeito_Morte`, `Prefab_Efeito_Construcao` (exemplos)
*   **Descrição:** Efeitos visuais curtos (faíscas, fumaça, brilho, pequena explosão) usados para dar feedback ao jogador sobre ações no jogo.
*   **Quando são utilizados:** Instanciados em locais específicos quando um evento ocorre (projétil atinge, inimigo morre, torre é construída).
*   **Quais seus componentes:**
    *   **Sprites / Particle System:** Principal componente para criar o visual do efeito.
    *   **Colisores:** Geralmente não são necessários.
    *   **Fontes de áudio:** Opcional, para um som diretamente ligado ao efeito visual.
    *   **Scripts:** Frequentemente um script simples para autodestruição.
*   **Descreva o comportamento dos scripts:**
    *   O script mais comum (`SelfDestruct`) simplesmente destrói o GameObject do efeito após um curto período de tempo (a duração da animação ou do sistema de partículas).

---

### 6. Prefab: Interface do Usuário (UI)

*   **Nome:** `Prefab_BotaoConstrucaoTorre`, `Prefab_BarraVidaInimigo` (exemplos)
*   **Descrição:** Elementos reutilizáveis da interface gráfica do jogador, como botões no menu de construção ou barras de vida que flutuam sobre os inimigos.
*   **Quando são utilizados:** Instanciados como parte do Canvas da UI do jogo. Botões podem ser fixos, barras de vida são criadas junto com os inimigos e os seguem.
*   **Quais seus componentes:**
    *   **Sprites:** Usados por componentes de UI como `Image` ou `Button`.
    *   **Colisores:** Não usam colisores 2D/3D padrão, mas sim componentes de UI como `Button` que detectam cliques/toques.
    *   **Fontes de áudio:** Podem ser usados em botões para som de clique.
    *   **Scripts:** Para definir o comportamento do elemento de UI.
*   **Descreva o comportamento dos scripts:**
    *   Scripts em botões respondem a cliques, ativando funções no jogo (ex: entrar no modo de posicionamento de torre).
    *   Scripts em barras de vida atualizam a exibição (o preenchimento da barra) com base na vida atual do inimigo associado.

---










### Prefabs
- Nome
- Descrição
- Quando são utilizados
- Quais seus componentes
    - Sprites
    - Colisores
    - Fontes de audio
    - Scripts
        - descreva o comportamento dos scripts
