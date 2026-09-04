<img src="https://capsule-render.vercel.app/api?type=waving&color=0:2E86AB,100:A23B72&height=200&section=header&text=Gestor%20de%20Avalia%C3%A7%C3%B5es&fontSize=42&fontColor=ffffff&animation=fadeIn&fontAlignY=35&desc=Sistema%20de%20gest%C3%A3o%20de%20tarefas%2C%20grupos%20e%20pautas&descAlignY=55&descSize=16" width="100%"/>

<div align="center">

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-WPF-239120?logo=csharp&logoColor=white)
![MVVM](https://img.shields.io/badge/Padrão-MVVM-blue)

</div>

## Sobre o Projeto

Aplicação desktop desenvolvida em **WPF (.NET 9.0)** para a unidade curricular de **Laboratório de Planeamento e Desenvolvimento de Software**, do curso de **Licenciatura em Engenharia Informática** (UTAD, 2024/2025).

A aplicação permite a um professor gerir o processo de avaliação de uma turma: registo de alunos, organização em grupos de trabalho, definição de tarefas de avaliação com respetiva ponderação, lançamento de classificações e consulta da pauta final, incluindo visualização gráfica dos resultados.

---

## Funcionalidades

- **Gestão de Perfil** — edição do nome, email e fotografia do utilizador (professor)
- **Gestão de Alunos (CRUD)** — número, nome e email
- **Importação de Alunos** a partir de ficheiro `.csv` ou `.xlsx`
- **Gestão de Tarefas de Avaliação (CRUD)** — título, descrição, datas de início/término e ponderação
- **Gestão de Grupos (CRUD)** — associação de alunos a grupos de trabalho
- **Lançamento de Resultados** por grupo (com possibilidade de exceções por aluno)
- **Consulta de Pauta** em formato de matriz, com a nota final calculada por aluno
- **Histograma** da distribuição de notas da turma

---

## Tecnologias Utilizadas

| Tecnologia | Descrição |
|---|---|
| **C#** | Linguagem de programação |
| **WPF (.NET 9.0)** | Framework de interface gráfica |
| **MVVM** | Padrão arquitetural (Model-View-ViewModel) |
| **XML / JSON** | Persistência de dados local |
| **Visual Studio 2022** | IDE de desenvolvimento |

---

## Arquitetura

A aplicação segue o padrão **MVVM**, separando claramente:

- **Model** — entidades de domínio (`Utilizador`, `Turma`, `TurmaAtual`, `Aluno`, `Grupo`, `Tarefa`, `LinhaPauta`)
- **View** — interfaces XAML
- **ViewModel** — lógica de apresentação e ligação (binding) entre Model e View

Os dados são persistidos em ficheiros XML/JSON, armazenados na pasta pessoal do perfil do utilizador Windows.

---

## Como Executar

1. Clonar o repositório
   ```bash
   git clone https://github.com/Tiago095/GestaoTurmas.git
   ```
2. Abrir a solução `.sln` no **Visual Studio 2022**
3. Restaurar os pacotes NuGet (automático ao abrir)
4. Definir o projeto WPF como *Startup Project*
5. Executar (`F5`)

**Pré-requisitos:** .NET 9.0 SDK e Visual Studio 2022

<img src="https://capsule-render.vercel.app/api?type=waving&color=0:2E86AB,100:A23B72&height=100&section=footer&animation=fadeIn&reversal=true" width="100%"/>
