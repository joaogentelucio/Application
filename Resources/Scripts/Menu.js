const menuIcon = document.getElementById('menu-icon');
const aside = document.querySelector('aside');
const imgIcon = document.getElementById('img');
const menuItems = document.querySelectorAll('nav ul li a');
const body = document.querySelector('body');

// Função para comportamento em telas grandes
function comportamentoTelasGrandes() {
    if (aside.classList.contains('expanded')) {
        aside.style.width = '10%';
        imgIcon.style.display = 'block';
    } else {
        aside.style.width = '4%';
        imgIcon.style.display = 'none';
    }
}

// Função para comportamento em telas pequenas
function comportamentoTelasPequenas() {
    if (aside.classList.contains('expanded')) {
        aside.style.width = '18%';
        imgIcon.style.display = 'block';
    } else {
        aside.style.width = '5%';
        imgIcon.style.display = 'none';
    }
}

// Função para ajustar a largura com base no tamanho da tela
function ajustarLargura() {
    if (window.innerWidth < 800) {
        comportamentoTelasPequenas();
    } else {
        comportamentoTelasGrandes();
    }
}

// Evento de clique no ícone do menu
menuIcon.addEventListener('click', () => {
    aside.classList.toggle('expanded');
    ajustarLargura();

    // Troca o ícone
    if (aside.classList.contains('expanded')) {
        menuIcon.classList.replace('fa-chevron-right', 'fa-chevron-left');
    } else {
        menuIcon.classList.replace('fa-chevron-left', 'fa-chevron-right');
    }
});

// Evento de clique nos itens do menu
menuItems.forEach(item => {
    item.addEventListener('click', () => {
        if (aside.classList.contains('expanded')) {
            aside.classList.remove('expanded');
            ajustarLargura();  // Ajusta a largura da sidebar após voltar ao normal

            // Troca o ícone de menu de volta para a barra
            menuIcon.classList.replace('fa-chevron-left', 'fa-chevron-right');
        }
    });
});

// Evento para detectar clique fora da sidebar
document.addEventListener('click', (event) => {
    if (!aside.contains(event.target) && !menuIcon.contains(event.target) && aside.classList.contains('expanded')) {
        // Se o clique for fora da sidebar e o menu estiver expandido, recolhe o menu
        aside.classList.remove('expanded');
        ajustarLargura();  // Ajusta a largura após recolher a sidebar

        // Troca o ícone de volta para a barra
        menuIcon.classList.replace('fa-xmark', 'fa-bars');
    }
});


// Ajusta a largura ao redimensionar a tela
window.addEventListener('resize', ajustarLargura);

// Configuração inicial
ajustarLargura();
