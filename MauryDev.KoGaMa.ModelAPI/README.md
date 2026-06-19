# KoGaMa Model API

A **KoGaMa Model API** é uma biblioteca leve, desenvolvida em `.NET Standard 2.0`, projetada para a manipulação, processamento e serialização de dados de modelos 3D do KoGaMa. Ela abstrai a complexidade de representar blocos (voxels) que possuem deformações de vértices, materiais por face e coordenadas compactadas.

## 🚀 Funcionalidades

- **Representação de Cubos**: Modelagem completa de blocos, incluindo posição global, materiais individuais para cada face e offsets de vértices (corners).
- **Gerenciamento de Modelos**: O `ModelInfo` agora atua como um gestor, permitindo adicionar, remover, buscar cubos por posição/material e calcular a *Bounding Box* do modelo.
- **Conversão de Coordenadas Bidirecional**: O `PositionConverter` traduz bytes compactados em `Vector3` e vice-versa, utilizando um sistema de grade $5 \times 5 \times 5$.
- **Estrutura de Dados Otimizada**: Uso de `IntVector` (baseado em `short`) para reduzir a pegada de memória em modelos com milhares de cubos.
- **Interface de Serialização**: `ISerializer` genérica para implementação de persistência em formatos Binários, JSON ou customizados.
- **Compatibilidade Máxima**: Alvo `.NET Standard 2.0` (compatível com Unity, .NET Framework e .NET Core/5+).

## 🛠 Arquitetura Técnica

### 1. `ModelInfo` (O Gestor do Modelo)
Não é apenas um container, mas a classe central de manipulação do modelo:
- **Busca e Filtro**: Métodos como `FindCubeAt(position)` e `GetCubesByMaterial(id)`.
- **Bounding Box**: Calcula automaticamente os limites mínimos e máximos do modelo através do método `GetBoundingBox()`.
- **Manipulação**: Suporte a `AddCube`, `RemoveCube` e `Clear`.

### 2. `Cube` (A Unidade Básica)
Representa um bloco individual com as seguintes propriedades:
- **`Position`**: Localização no grid via `IntVector`.
- **`Materials`**: Array de 6 bytes (um para cada face definida no enum `Face`).
- **`Corners`**: Array de 8 bytes que definem a deformação dos vértices. 
    - *Dica:* Use `Cube.IdentityByteCorners` para resetar um cubo ao seu estado padrão.
- **`GetCornersVectors()`**: Converte os bytes de corners em `Vector3` reais para renderização.

### 3. `PositionConverter` (O Coração Matemático)
Mapeia um único `byte` (0-124) para um `Vector3` baseado em uma grade de 5 níveis:
- **Valores de referência**: `{-0.5f, -0.25f, 0f, 0.25f, 0.5f}`.
- **Cálculo**:
  - $X = index / 25
  - $Y = (index % 25) / 5
  - $Z = index % 5

### 4. `IntVector`
Uma estrutura `struct` otimizada para coordenadas inteiras, evitando o overhead de `float` quando não necessário e provendo acesso via indexador (`vector[0]` para X, etc).

---

## 💻 Exemplos de Uso

### Criando e Gerenciando um Modelo
```csharp
using MauryDev.KoGaMa.ModelAPI.Model;
using MauryDev.KoGaMa.ModelAPI.Models;

var model = new ModelInfo();

// Criando um cubo na posição (0, 1, 0)
var cube = new Cube(new IntVector(0, 1, 0)) 
{
    Materials = new byte[] { 1, 1, 1, 1, 1, 1 } 
};

model.AddCube(cube);

// Verificando se existe um cubo em certa posição
if (model.HasCubeAt(new IntVector(0, 1, 0))) 
{
    var foundCube = model.FindCubeAt(new IntVector(0, 1, 0));
}

// Obtendo a Bounding Box do modelo
var (min, max) = model.GetBoundingBox();
Console.WriteLine($"Modelo expande de {min} até {max}");
```

### Trabalhando com Vértices (Corners)
```csharp
var cube = new Cube(new IntVector(0, 0, 0));
cube.Corners = Cube.IdentityByteCorners; // Define corners padrão

// Obtém as posições reais dos 8 vértices para renderização
System.Numerics.Vector3[] vertices = cube.GetCornersVectors();

foreach (var v in vertices)
{
    Console.WriteLine($"Vértice: {v}");
}
```

### Conversão Manual de Coordenadas
```csharp
using MauryDev.KoGaMa.ModelAPI.Utils;

// De Byte para Vector3
byte byteVal = 124; 
System.Numerics.Vector3 vec = PositionConverter.GetVectorFromByte(byteVal);

// De Vector3 para Byte
byte backToByte = PositionConverter.GetByteFromVector(vec);
```

---

## 📦 Instalação

Adicione a referência à DLL compilada ao seu projeto ou inclua os arquivos fonte.

**Dependências:**
- `System.Numerics.Vectors` (v4.6.1+)

## 📐 Tabela de Coordenadas (Grid 5x5x5)

| Índice | Valor Float |
| :--- | :--- |
| 0 | -0.5f |
| 1 | -0.25f |
| 2 | 0f |
| 3 | 0.25f |
| 4 | 0.5f |

## 👤 Autor

- **MauryDev**

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.
