# G.MD — Guia de Especificação do Projeto
# MathPillars Explorer: A Anatomia Matemática da IA
# Versão: 1.0.0 | Criado em: 2026-04-25 | Status: Em Desenvolvimento

---

## 1. Visão Geral do Projeto

### 1.1 Identidade

- **Nome:** MathPillars Explorer: A Anatomia Matemática da IA
- **Tipo:** Laboratório interativo educacional de alta performance
- **Plataforma:** Web (.NET 10 — Blazor WebAssembly + ASP.NET Core)
- **Paradigma:** Stateless, orientado a experimentos, com embasamento teórico rigoroso

### 1.2 Propósito

Fornecer um ambiente interativo e matematicamente rigoroso para visualizar, formalizar e experimentar os três pilares matemáticos que sustentam os sistemas de Inteligência Artificial modernos:

- **Pilar A:** Álgebra Linear — o espaço de representação
- **Pilar B:** Cálculo Multivariável — o mecanismo de ajuste
- **Pilar C:** Probabilidade e Estatística — o gerenciamento da incerteza

### 1.3 Público-Alvo

| Perfil | Expectativa |
|--------|-------------|
| Profissional de ML/IA | Ferramenta de consulta e experimentação visual rápida |
| Professor e Pesquisador | Recurso de apoio para aulas e demonstrações |
| Autodidata Avançado | Compreensão profunda da matemática por trás de LLMs e redes neurais |

**Premissa universal:** Todos os experimentos possuem obrigatoriamente embasamento teórico com formalização em LaTeX. Nenhum módulo é apenas visual.

---

## 2. Decisões de Arquitetura (Decision Log)

| # | Decisão | Alternativas Consideradas | Justificativa |
|---|---------|--------------------------|---------------|
| D1 | Blazor WASM + ASP.NET Core Backend (opção C) | Blazor Server, Blazor Auto | Desacoplamento natural MathCore/VisualUI; backend focado em matemática; frontend estático |
| D2 | Todos os módulos entregues simultaneamente | Entrega faseada por módulo | Usuário quer validação completa do sistema |
| D3 | Sistema 100% stateless | localStorage, contas de usuário | Foco total na ferramenta; zero overhead de infraestrutura de dados |
| D4 | Processamento assíncrono com SSE | REST síncrono, WebSockets, SignalR | Cálculos pesados não bloqueiam UI; progresso em tempo real sem overhead de WebSocket |
| D5 | Vertical Slice Architecture | Clean Architecture horizontal, projetos NuGet internos | Mapeamento 1:1 com os 3 módulos; desenvolvimento paralelo real; sem abstração excessiva |

---

## 3. Stack Tecnológica

### 3.1 Backend — MathPillars.Api

| Tecnologia | Versão | Finalidade |
|------------|--------|------------|
| .NET | 10 | Runtime principal |
| ASP.NET Core | 10 | Framework Web API |
| MathNet.Numerics | Latest | Álgebra linear, decomposições, estatística |
| IAsyncEnumerable | Nativo .NET | Streaming SSE de resultados parciais |

### 3.2 Frontend — MathPillars.Web

| Tecnologia | Versão | Finalidade |
|------------|--------|------------|
| Blazor WebAssembly | .NET 10 | Framework de UI |
| KaTeX / MathJax | Latest | Renderização de fórmulas LaTeX |
| Plotly.NET | Latest | Visualização 3D (Loss Landscape, espaços vetoriais) |
| ScottPlot | Latest | Visualização 2D (distribuições, gradientes) |

### 3.3 Testes — MathPillars.Testes

| Tecnologia | Finalidade |
|------------|------------|
| xUnit | Framework de testes unitários |
| FluentAssertions | Assertivas legíveis e expressivas |

---

## 4. Arquitetura do Sistema

### 4.1 Estrutura da Solução

```
MathPillarsExplorer.sln
├── src/
│   ├── MathPillars.Api/
│   └── MathPillars.Web/
└── testes/
    └── MathPillars.Testes/
```

### 4.2 Fluxo de Comunicação

```
[Usuário no Browser]
        │
        ▼
[MathPillars.Web — Blazor WASM]
  Renderização: KaTeX, Plotly.NET, ScottPlot
  Componentes: PainelTeoria, ProgressoSSE, PainelParametros
        │
        ├── POST  JSON  ──────────────────────────► [MathPillars.Api]
        │                                              Cálculos leves (< 500ms)
        │
        └── GET   SSE (text/event-stream)  ─────────► [MathPillars.Api]
            ◄── stream de ResultadoSSE<T> ───────────  Cálculos pesados (SVD, PCA, Loss Landscape)
```

### 4.3 Estrutura Interna — MathPillars.Api

```
MathPillars.Api/
├── Modulos/
│   ├── AlgebraLinear/
│   │   ├── AlgebraLinearControlador.cs
│   │   ├── ProdutoEscalarServico.cs
│   │   ├── SimilaridadeCossenoServico.cs
│   │   ├── SVDServico.cs
│   │   ├── PCAServico.cs
│   │   └── AutovetoresServico.cs
│   ├── CalculoMultivariavel/
│   │   ├── CalculoControlador.cs
│   │   ├── GradienteServico.cs
│   │   ├── JacobianaServico.cs
│   │   ├── LossLandscapeServico.cs
│   │   └── ComparadorOtimizadoresServico.cs
│   └── Probabilidade/
│       ├── ProbabilidadeControlador.cs
│       ├── BayesServico.cs
│       ├── GaussianaServico.cs
│       └── EntropiaCruzadaServico.cs
├── Compartilhado/
│   └── Primitivos/
│       ├── Vetor.cs
│       ├── Matriz.cs
│       ├── Escalar.cs
│       └── ResultadoSSE.cs
└── Program.cs
```

### 4.4 Estrutura Interna — MathPillars.Web

```
MathPillars.Web/
├── Paginas/
│   ├── Inicio.razor
│   ├── AlgebraLinear/
│   │   ├── AlgebraLinearPagina.razor
│   │   ├── ExperimentoProdutoEscalar.razor
│   │   ├── ExperimentoSimilaridadeCosseno.razor
│   │   ├── ExperimentoSVD.razor
│   │   └── ExperimentoPCA.razor
│   ├── CalculoMultivariavel/
│   │   ├── CalculoPagina.razor
│   │   ├── ExperimentoLossLandscape.razor
│   │   ├── ExperimentoOtimizadores.razor
│   │   └── ExperimentoBackpropagation.razor
│   └── Probabilidade/
│       ├── ProbabilidadePagina.razor
│       ├── ExperimentoBayes.razor
│       ├── ExperimentoGaussiana.razor
│       └── ExperimentoEntropiaCruzada.razor
├── Componentes/
│   ├── PainelTeoria.razor
│   ├── GraficoPlotly3D.razor
│   ├── GraficoScottPlot2D.razor
│   ├── ProgressoSSE.razor
│   └── PainelParametros.razor
└── Servicos/
    ├── ClienteMathCore.cs
    └── LeitorSSE.cs
```

---

## 5. Contrato SSE — Padrão de Streaming

Todos os endpoints de cálculo pesado emitem eventos no seguinte formato:

### Modelo de Dados

```csharp
public record ResultadoSSE<T>(
    int ProgressoPercentual,
    string MensagemProgresso,
    T? DadosParciais,
    bool Concluido
);
```

### Formato do Stream

```
data: {"progressoPercentual":0,"mensagemProgresso":"Iniciando decomposição...","dadosParciais":null,"concluido":false}

data: {"progressoPercentual":33,"mensagemProgresso":"Calculando matriz U...","dadosParciais":null,"concluido":false}

data: {"progressoPercentual":66,"mensagemProgresso":"Calculando valores singulares Σ...","dadosParciais":null,"concluido":false}

data: {"progressoPercentual":100,"mensagemProgresso":"Concluído.","dadosParciais":{...},"concluido":true}
```

---

## 6. Módulos — Especificação Detalhada

### 6.1 Módulo A: Álgebra Linear — O Espaço de Representação

#### Conceitos Cobertos

- Tensores de ordens 0 a N
- Produto escalar e projeção vetorial
- Similaridade de cosseno
- Decomposição SVD: $A = U\Sigma V^T$
- Decomposição PCA: redução de dimensionalidade
- Autovetores e autovalores: $Av = \lambda v$

#### Formalizações LaTeX Obrigatórias

$$\text{similarity}(A, B) = \frac{A \cdot B}{\|A\| \|B\|} = \cos(\theta)$$

$$A = U\Sigma V^T \quad \text{onde } U, V \text{ são ortogonais e } \Sigma \text{ é diagonal}$$

$$Av = \lambda v \quad \text{onde } \lambda \text{ é autovalor e } v \text{ é autovetor}$$

#### Endpoints

| Método | Rota | Tipo | Descrição |
|--------|------|------|-----------|
| POST | `/api/algebra-linear/produto-escalar` | Síncrono | Calcula produto escalar de dois vetores |
| POST | `/api/algebra-linear/similaridade-cosseno` | Síncrono | Calcula similaridade entre dois vetores |
| POST | `/api/algebra-linear/autovetores` | Síncrono | Calcula autovetores e autovalores de matriz quadrada |
| GET | `/api/algebra-linear/svd/stream` | SSE | Decomposição SVD com progresso em tempo real |
| GET | `/api/algebra-linear/pca/stream` | SSE | PCA com animação da redução dimensional |

#### Experimentos Interativos

| # | Nome | Ação do Usuário | Saída do Sistema |
|---|------|-----------------|------------------|
| A1 | Produto Escalar e Projeção | Inserir dois vetores manualmente | Resultado numérico + gráfico 2D da projeção + definição formal |
| A2 | Similaridade de Cosseno Semântica | Inserir pares de vetores (ex: representações de palavras) | Ângulo entre vetores + gráfico 3D no espaço latente + fórmula KaTeX |
| A3 | Decomposição SVD | Inserir matriz A (até 10x10) | U, Σ, Vᵀ via SSE + visualização dos valores singulares em gráfico de barras |
| A4 | PCA Interativo | Carregar dataset sintético 2D ou 3D | Redução de dimensionalidade animada + variância explicada por componente |

---

### 6.2 Módulo B: Cálculo Multivariável — O Mecanismo de Ajuste

#### Conceitos Cobertos

- Derivadas parciais e gradiente $\nabla f$
- Regra da cadeia e Backpropagation
- Matriz Jacobiana: $J_{ij} = \frac{\partial f_i}{\partial x_j}$
- Matriz Hessiana: $H_{ij} = \frac{\partial^2 f}{\partial x_i \partial x_j}$
- Otimizadores de 1ª ordem (AdamW) e 2ª ordem (Sophia)

#### Formalizações LaTeX Obrigatórias

$$\nabla f(x) = \left(\frac{\partial f}{\partial x_1}, \frac{\partial f}{\partial x_2}, \ldots, \frac{\partial f}{\partial x_n}\right)$$

$$\theta_{t+1} = \theta_t - \eta \cdot \nabla_\theta \mathcal{L}(\theta_t) \quad \text{(Gradient Descent)}$$

$$\theta_{t+1} = \theta_t - \eta \cdot \text{clip}\left(\frac{m_t}{\max\{\gamma \cdot h_t, \epsilon\}}, 1\right) \quad \text{(Sophia)}$$

$$J = \begin{bmatrix} \frac{\partial f_1}{\partial x_1} & \cdots & \frac{\partial f_1}{\partial x_n} \\ \vdots & \ddots & \vdots \\ \frac{\partial f_m}{\partial x_1} & \cdots & \frac{\partial f_m}{\partial x_n} \end{bmatrix}$$

#### Endpoints

| Método | Rota | Tipo | Descrição |
|--------|------|------|-----------|
| POST | `/api/calculo/gradiente` | Síncrono | Calcula gradiente de função paramétrica |
| POST | `/api/calculo/jacobiana` | Síncrono | Calcula matriz Jacobiana |
| GET | `/api/calculo/loss-landscape/stream` | SSE | Gera superfície 3D de Loss Landscape |
| GET | `/api/calculo/otimizador/comparar/stream` | SSE | Compara trajetórias AdamW vs Sophia |

#### Experimentos Interativos

| # | Nome | Ação do Usuário | Saída do Sistema |
|---|------|-----------------|------------------|
| B1 | Loss Landscape 3D | Escolher função de perda e ajustar parâmetros | Superfície 3D renderizada via SSE + vetor gradiente sobreposto |
| B2 | Comparador AdamW vs Sophia | Definir learning rate e configurações de curvatura | Trajetória de ambos os otimizadores na Loss Landscape + análise comparativa da Hessiana |
| B3 | Backpropagation Visual | Definir rede simples (2 a 3 camadas) | Fluxo da regra da cadeia etapa a etapa com formalização LaTeX em cada nó |

---

### 6.3 Módulo C: Probabilidade e Estatística — O Gerenciamento da Incerteza

#### Conceitos Cobertos

- Teorema de Bayes
- Distribuições Gaussianas e modelagem de ruído
- Entropia e Cross-Entropy Loss
- Prior, Likelihood e Posterior

#### Formalizações LaTeX Obrigatórias

$$P(A|B) = \frac{P(B|A) \cdot P(A)}{P(B)}$$

$$f(x|\mu,\sigma^2) = \frac{1}{\sigma\sqrt{2\pi}} e^{-\frac{(x-\mu)^2}{2\sigma^2}}$$

$$H(P, Q) = -\sum_{x} P(x) \log Q(x) \quad \text{(Cross-Entropy)}$$

$$H(P) = -\sum_{x} P(x) \log P(x) \quad \text{(Entropia de Shannon)}$$

#### Endpoints

| Método | Rota | Tipo | Descrição |
|--------|------|------|-----------|
| POST | `/api/probabilidade/bayes` | Síncrono | Computa posterior dado prior e likelihood |
| POST | `/api/probabilidade/entropia-cruzada` | Síncrono | Calcula Cross-Entropy entre distribuições P e Q |
| POST | `/api/probabilidade/distribuicao-normal` | Síncrono | Gera pontos de distribuição normal com μ e σ |
| GET | `/api/probabilidade/gaussiana/stream` | SSE | Ajusta curva gaussiana dinamicamente a pontos de dados |

#### Experimentos Interativos

| # | Nome | Ação do Usuário | Saída do Sistema |
|---|------|-----------------|------------------|
| C1 | Teorema de Bayes Interativo | Ajustar P(A), P(B\|A) e P(B) via sliders | Diagrama de transição prior→posterior atualizado em tempo real + fórmula KaTeX |
| C2 | Gaussiana Dinâmica | Inserir pontos de dados ou ajustar μ e σ manualmente | Curva normal se reconstruindo via SSE + overlay dos pontos inseridos |
| C3 | Cross-Entropy Loss | Comparar duas distribuições P e Q | Valor numérico de entropia + gráfico de divergência + fórmula KaTeX |

---

## 7. Seção Teórica: Fronteiras de 2026

### 7.1 Raciocínio de Longo Horizonte em Sistemas AGI

Esta seção explica como a combinação dos três pilares matemáticos permite que modelos de larga escala (como GPT-5.x e Gemini 3) realizem raciocínio de longo horizonte:

- **Álgebra Linear** fornece o espaço de representação onde conceitos coexistem como vetores
- **Cálculo Multivariável** permite que o modelo ajuste seus pesos para minimizar erros ao longo de trilhões de parâmetros
- **Probabilidade** permite que o modelo gerencie incerteza e produza saídas calibradas

### 7.2 Framework LEA — Lógica, Emoção, Autonomia

Discussão teórica sobre como a matemática sustenta a estabilidade de sistemas AGI:

- **Lógica:** Operações matriciais e decomposições como base do raciocínio simbólico
- **Emoção:** Distribuições probabilísticas como modelo de preferência e aversão ao risco
- **Autonomia:** Otimizadores de 2ª ordem (Sophia, Newton) como mecanismo de tomada de decisão autônoma

---

## 8. Requisitos Não-Funcionais

| Requisito | Definição | Estratégia |
|-----------|-----------|------------|
| **Performance** | Cálculos leves < 500ms; pesados via SSE sem timeout de UI | `IAsyncEnumerable` + cancellation tokens |
| **Escalabilidade** | Sessões independentes, sem estado compartilhado | Stateless por design; sem banco de dados |
| **Segurança** | Inputs matemáticos validados para evitar DoS | Limites máximos: matrizes até 1000x1000; datasets até 10.000 pontos |
| **Confiabilidade** | Cálculos matematicamente corretos e verificáveis | Cobertura de testes > 80% no MathCore via xUnit |
| **Manutenibilidade** | Módulos independentes, evolutivos | Vertical Slice Architecture — sem acoplamento entre módulos |

---

## 9. Premissas Registradas

| # | Premissa |
|---|----------|
| P1 | Dados de entrada são sintéticos ou inseridos manualmente — sem upload de arquivos externos |
| P2 | Não há integração com modelos reais de ML (GPT, Gemini) — apenas simulações matemáticas |
| P3 | A seção Fronteiras de 2026 e Framework LEA é informacional e teórica, sem interatividade |
| P4 | Matrizes têm limite máximo de 1000x1000 e datasets de até 10.000 pontos para evitar sobrecarga |
| P5 | Deploy inicial em ambiente local de desenvolvimento; hospedagem em cloud é objetivo futuro |

---

## 10. Roadmap de Implementação

### Fase 0 — Infraestrutura Base

- [ ] Criar solução `.sln` com projetos `Api`, `Web` e `Testes`
- [ ] Configurar CORS na API para aceitar requisições do WASM
- [ ] Implementar `ResultadoSSE<T>`, `Vetor`, `Matriz`, `Escalar` em `Compartilhado/`
- [ ] Implementar `LeitorSSE.cs` no Web para consumo de `text/event-stream`
- [ ] Integrar KaTeX no Blazor WASM via JS Interop
- [ ] Criar wrappers `GraficoPlotly3D.razor` e `GraficoScottPlot2D.razor`
- [ ] Criar `ClienteMathCore.cs` com `HttpClient` configurado
- [ ] Criar `PainelTeoria.razor` e `ProgressoSSE.razor`
- [ ] Instalar `MathNet.Numerics` na API
- [ ] Configurar pipeline de testes com xUnit e FluentAssertions

### Fase 1 — Módulo A: Álgebra Linear

- [ ] Implementar `ProdutoEscalarServico` e `SimilaridadeCossenoServico`
- [ ] Implementar `SVDServico` com streaming SSE via `IAsyncEnumerable`
- [ ] Implementar `PCAServico` com streaming SSE
- [ ] Implementar `AutovetoresServico`
- [ ] Criar `AlgebraLinearControlador` com todos os endpoints
- [ ] Criar `ExperimentoProdutoEscalar.razor`
- [ ] Criar `ExperimentoSimilaridadeCosseno.razor`
- [ ] Criar `ExperimentoSVD.razor`
- [ ] Criar `ExperimentoPCA.razor`
- [ ] Escrever testes unitários para todos os serviços matemáticos do módulo

### Fase 2 — Módulo B: Cálculo Multivariável

- [ ] Implementar `GradienteServico` e `JacobianaServico`
- [ ] Implementar `LossLandscapeServico` com SSE (superfície 3D)
- [ ] Implementar `ComparadorOtimizadoresServico` (AdamW e Sophia)
- [ ] Criar `CalculoControlador` com todos os endpoints
- [ ] Criar `ExperimentoLossLandscape.razor` com Plotly 3D
- [ ] Criar `ExperimentoOtimizadores.razor` com comparação visual de trajetórias
- [ ] Criar `ExperimentoBackpropagation.razor` com fluxo da regra da cadeia
- [ ] Escrever testes unitários para todos os serviços matemáticos do módulo

### Fase 3 — Módulo C: Probabilidade e Estatística

- [ ] Implementar `BayesServico`
- [ ] Implementar `GaussianaServico` com SSE
- [ ] Implementar `EntropiaCruzadaServico`
- [ ] Criar `ProbabilidadeControlador` com todos os endpoints
- [ ] Criar `ExperimentoBayes.razor` com sliders interativos
- [ ] Criar `ExperimentoGaussiana.razor` com SSE
- [ ] Criar `ExperimentoEntropiaCruzada.razor`
- [ ] Escrever testes unitários para todos os serviços matemáticos do módulo

### Fase 4 — Seção Teórica + Polimento Final

- [ ] Criar página teórica sobre Raciocínio de Longo Horizonte
- [ ] Criar seção Framework LEA com embasamento matemático formal
- [ ] Revisão geral de UX, navegação entre módulos e responsividade
- [ ] Garantir cobertura de testes > 80% em todo o MathCore
- [ ] Criar todos os 13 diagramas UML e 4 diagramas C4 na pasta `analise/`
- [ ] Escrever `README.md` completo e formal

---

## 11. Estado Atual do Desenvolvimento

| Fase | Status |
|------|--------|
| Fase 0 — Infraestrutura Base | Concluída (100%) |
| Fase 1 — Módulo A: Álgebra Linear | Concluída (100%) |
| Fase 2 — Módulo B: Cálculo Multivariável | Concluída (100%) |
| Fase 3 — Módulo C: Probabilidade e Estatística | Concluída (100%) |
| Fase 4 — Seção Teórica + Polimento | Concluída (100%) |

---

*Este documento é o guia mestre do projeto. Deve ser atualizado a cada decisão arquitetural relevante, mudança de escopo ou conclusão de fase.*
