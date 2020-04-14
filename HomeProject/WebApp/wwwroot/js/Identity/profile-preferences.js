let genderInput = document.getElementById('Input_ProfileGender');
let genderOwn = document.getElementById('profile-gender-own');

// console.log(genderInput);
// console.log(genderOwn);
if (parseInt(genderInput.value) === 127){
    genderOwn.classList.remove('d-none')
}

genderInput.addEventListener("change", ev => {
    console.log(genderInput.value);
    if (parseInt(genderInput.value) === 127){
        genderOwn.classList.remove('d-none')
    }
    else{
        genderOwn.classList.add('d-none')
    }
});