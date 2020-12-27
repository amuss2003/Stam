pipeline {
  agent any
  stages {
    stage("build") {
      steps {
        echo 'building'
        script {
          docker.build amirimage123
        }
      }
    }
    stage("test") {
      steps {
        echo 'testing'
      }
    }
    stage("deploy") {
      steps {
        echo 'deploying'
      }
    }
  }
}
