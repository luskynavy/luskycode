<template>
  <ScoreBoard :winCount="this.winCount" :loseCount="this.loseCount" />

  <template v-if="this.question">
  <div>
      <h1 v-html="this.question">
      </h1>

      <template v-for="answer in this.answers" :key="answer">
        <input
         type="radio"
         name="options"
          :value="answer"
          v-model="chosenAnswer"
          :disabled="this.answerSubmitted">
        <label v-html="answer"></label><br>
      </template>

      <button v-if="!this.answerSubmitted" @click="this.submitAnswer" class="send" type="button">Send</button>

      <section v-if="this.answerSubmitted" class="result">
        <h4 v-if="this.chosenAnswer != this.correctAnswer"
          v-html="'&#10060; Wrong answer, correct answer is ' + this.correctAnswer + '.'"></h4>
        <h4 v-else
        v-html="'&#9989; Correct answer.'"></h4>
        <button @click="this.getNewQuestion()" class="send" type="button">Next question</button>
      </section>
    </div>
  </template>
</template>

<script>
import ScoreBoard from './components/ScoreBoard.vue';

export default {
  name: 'App',
  components: {
    ScoreBoard
  },

  data() {
    return {
      question: undefined,
      incorrectAnswers: undefined,
      correctAnswer: undefined,
      chosenAnswer: undefined,
      answerSubmitted: false,
      winCount: 0,
      loseCount: 0
    }
  },

  computed: {
    answers() {
      //copy incorrectAnswers
      var answers = [...this.incorrectAnswers];
      //Put correct answer to a random position
      answers.splice(Math.round(Math.random() * this.incorrectAnswers.length), 0, this.correctAnswer);
      return answers;
    }
  },

  methods: {
    submitAnswer() {
      if (!this.chosenAnswer) {
          alert('Pick one the options')
      } else {
        this.answerSubmitted = true;
        if(this.chosenAnswer == this.correctAnswer) {
          this.winCount++;
        } else {
          this.loseCount++;
        }
      }
    },
    getNewQuestion() {
      this.answerSubmitted = false;
      this.chosenAnswer = undefined;
      this.question = undefined;

      this.axios
        .get('https://opentdb.com/api.php?amount=1&difficulty=easy&category=18')
        .then((response) => {
          this.question = response.data.results[0].question
          this.incorrectAnswers = response.data.results[0].incorrect_answers
          this.correctAnswer = response.data.results[0].correct_answer
        })
        .catch(err => {alert('Error, trying again.\r' + err) ; this.getNewQuestion()})
    }
  },

  created() {
    this.getNewQuestion()
  }
}

</script>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin: 60px auto;
  max-width: 960px;

  input[type=radio] {
    margin: 12px 4px;
  }

  button.send {
    margin-top: 12px;
    height: 40px;
    min-width: 120px;
    padding: 0 16px;
    color: #fff;
    background-color: #1867c0;
    border: 1px solid #1867c0;
    cursor: pointer;
  }
}
</style>
