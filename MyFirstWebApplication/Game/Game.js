var arr = new Array();

var numValue;

function myFunction(){
  numValue = document.getElementById('txtNum').value;
}

function makeid() {
  var text = "";
  var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789*/";

  for (var i = 0; i < 10; i++)
    text += possible.charAt(Math.floor(Math.random() * possible.length));

  return text;
}

console.log(makeid());

$('.next-ball').on('click', function(){
  var random = Math.floor(Math.random()*2);
  console.log(random);
  
  if(arr.indexOf(random) > -1 && random === 1)
    {
      random = 0;
    }
  arr.push(random);
  
  var index = arr.indexOf(random) + 1;
  
  if(random === 1)
    {
      $('.lottery').append('<li class="plate">' + '</li>');
      
    }
  else
    {
      $('.lottery').append('<li class="lottery-ball">' + '</li>');
    } 
  
  console.log(arr);
  
  if ((index == numValue) && (random === 1)){
     alert('Угадали!' +'\n' + 'Для получения скидки предоставьте этот код продавцу' + '\n' + makeid());
  }
  
  if ( $('.lottery').children().length > 5 ) {
    $('.next-ball').hide();
    $('.play-again').show();
  }

  
});

$('.play-again').on('click', function(){
  $('.lottery').children().remove();
  arr = [];
  $('.next-ball').show();
  $('.play-again').hide();
});
