def RelevantChangesFound() {
  def changeLogSets = currentBuild.changeSets
  for (int i = 0; i < changeLogSets.size(); i++) {
      for (int j = 0; j < changeLogSets[i].items.length; j++) {
          if ("${changeLogSets[i].items[j].author}" != "noreply")
            return true;
      }
  }
  return false;
}

pipeline {
  agent any
  triggers {
    pollSCM('H/2 * * * *') // Every 2 minutes
  }
  environment {
    ACR_URL = 'amircontainerregistry.azurecr.io'
  }
  parameters {
    choice(name: 'BuildOptions', choices: ['Only if git changes occured', 'Force', 'Skip'], description: 'Build options')
    string(name: 'ManualDeployImage', defaultValue: '', description: 'Manually deploy image name (leave blank to skip deploy)')
  }
  stages {
    stage("build") {
      when {
        expression {
          params.BuildOptions == 'Force' || 
            (params.BuildOptions == 'Only if git changes occured' && RelevantChangesFound())
        }
      }
      steps {      
        script {
          env.CONTAINER_IMAGE_NAME = "${ACR_URL}/stable/restapi:${BUILD_TIMESTAMP}"
          echo "building image ${CONTAINER_IMAGE_NAME}"
          withCredentials([usernamePassword(credentialsId: 'ACR', usernameVariable: 'ACR_USER', passwordVariable: 'ACR_PASSWORD')]) {
            sh "docker login -u $ACR_USER -p $ACR_PASSWORD https://${ACR_URL}"
            def image = docker.build "${CONTAINER_IMAGE_NAME}"
            image.push()
          }
        }
      }
    }
    stage("deploy") {
      when {
        expression {
          env.CONTAINER_IMAGE_NAME || params.ManualDeployImage != ''
        }
      }
      steps {
        script {
          if (params.ManualDeployImage != '')
            env.CONTAINER_IMAGE_NAME = params.ManualDeployImage
          
          echo "deploying image ${CONTAINER_IMAGE_NAME}"
          kubernetesDeploy(
              configs: 'azure-testapp.yaml',
              kubeconfigId: 'KubeConfig',
              enableConfigSubstitution: true
              )   
        }
      }
    }
  }
}
