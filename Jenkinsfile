pipeline {
  agent any
  stages {
    stage("build") {
      steps {
        echo 'building'
        withCredentials([usernamePassword(credentialsId: 'ACR', usernameVariable: 'ACR_USER', passwordVariable: 'ACR_PASSWORD')] {
          
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
