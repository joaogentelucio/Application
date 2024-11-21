const apiUrl = "https://localhost:44339/api/Usuarios/ListarUsuarios"; // URL da API que lista os usuários

document.addEventListener("DOMContentLoaded", async () => {
    const token = localStorage.getItem("token"); // Recupera o token JWT armazenado
    const userList = document.getElementById("user-list");



    try {
        // Requisição GET para a API de usuários
        const response = await fetch(apiUrl, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}` // Passa o token JWT no cabeçalho
            }
        });

        if (!response.ok) {
            const errorData = await response.json();
            console.error("Erro no servidor", errorData);
            throw new Error(errorData.message || "Erro ao carregar usuários");
        }

        const data = await response.json();
        
        // Preenche a lista de usuários com cards horizontais
        data.forEach(user => {
            const card = document.createElement('div');
            card.classList.add('card');
            card.innerHTML = `
                Nome: ${user.nome}
                <br />
                Email: ${user.email}
            `;
            userList.appendChild(card);
        });

    } catch (error) {
        console.error("Erro:", error);
    }
});
